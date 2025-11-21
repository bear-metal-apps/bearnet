using System.Net;
using System.Text.Json;
using Bearnet.Models.Cache;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Bearnet.Services;

public interface IApiClient {
    Task<string> GetStringAsync(string endpoint, TimeSpan? ttl = null);
    Task<T> GetAsync<T>(string endpoint, TimeSpan? ttl = null, JsonSerializerOptions? jsonOptions = null);
}

public abstract class ApiClientBase : IApiClient {
    private readonly HttpClient _httpClient;
    private readonly IMongoCollection<CachedResponse> _cache;
    private readonly ILogger _logger;
    private readonly TimeSpan _defaultTtl;
    private readonly string _apiName;
    private readonly Task _indexInitialization;

    protected ApiClientBase(
        string apiName,
        string httpClientName,
        IHttpClientFactory httpClientFactory,
        IMongoCollection<CachedResponse> cache,
        ILogger logger,
        TimeSpan? defaultTtl = null) {
        _apiName = apiName;
        _httpClient = httpClientFactory.CreateClient(httpClientName);
        _cache = cache;
        _logger = logger;
        _defaultTtl = defaultTtl ?? TimeSpan.FromMinutes(60);

        // Fire-and-forget index creation so cold starts aren't blocked.
        _indexInitialization = Task.Run(CreateIndexesAsync);
    }

    private async Task CreateIndexesAsync() {
        try {
            _logger.LogDebug("Creating cache indexes for {Api}", _apiName);
            var indexKeys = Builders<CachedResponse>.IndexKeys
                .Ascending(x => x.Api)
                .Ascending(x => x.Endpoint);
            await _cache.Indexes.CreateOneAsync(
                new CreateIndexModel<CachedResponse>(indexKeys, new CreateIndexOptions {
                    Unique = true,
                    Background = true
                })
            );

            var expiresIndex = Builders<CachedResponse>.IndexKeys.Ascending(x => x.ExpiresAt);
            await _cache.Indexes.CreateOneAsync(
                new CreateIndexModel<CachedResponse>(expiresIndex, new CreateIndexOptions {
                    ExpireAfter = TimeSpan.Zero,
                    Background = true
                })
            );
            _logger.LogDebug("Cache indexes created successfully for {Api}", _apiName);
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Error creating cache indexes (may already exist)");
        }
    }

    public async Task<string> GetStringAsync(string endpoint, TimeSpan? ttl = null) {
        await _indexInitialization.ConfigureAwait(false);

        var now = DateTimeOffset.UtcNow;
        _logger.LogDebug("Checking cache for {Api}:{Endpoint}", _apiName, endpoint);
        var cachedData = await _cache.Find(x => x.Api == _apiName && x.Endpoint == endpoint).FirstOrDefaultAsync();
        var effectiveTtl = ttl ?? _defaultTtl;

        if (cachedData != null && cachedData.ExpiresAt > now.UtcDateTime) {
            _logger.LogDebug("Cache hit for {Api}:{Endpoint}", _apiName, endpoint);
            return cachedData.Content;
        }

        _logger.LogDebug("Cache miss for {Api}:{Endpoint}", _apiName, endpoint);
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        if (!string.IsNullOrEmpty(cachedData?.ETag)) {
            request.Headers.TryAddWithoutValidation("If-None-Match", cachedData.ETag);
        }

        _logger.LogDebug("Making HTTP request to {Endpoint}", endpoint);
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotModified && cachedData != null) {
            _logger.LogDebug("Using cached data due to NotModified for {Api}:{Endpoint}", _apiName, endpoint);
            // Update cache expiration
            var filter = Builders<CachedResponse>.Filter.Where(x => x.Api == _apiName && x.Endpoint == endpoint);
            var update = Builders<CachedResponse>.Update
                .Set(x => x.ExpiresAt, now.Add(effectiveTtl).UtcDateTime)
                .Set(x => x.LastModified, now.UtcDateTime);
            await _cache.UpdateOneAsync(filter, update);
            return cachedData.Content;
        }

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var etag = response.Headers.ETag?.Tag;

        _logger.LogDebug("Received response with status {StatusCode} for {Endpoint}", response.StatusCode, endpoint);
        _logger.LogDebug("Caching response for {Api}:{Endpoint}", _apiName, endpoint);
        var cacheUpdate = Builders<CachedResponse>.Update
            .Set(x => x.Api, _apiName)
            .Set(x => x.Endpoint, endpoint)
            .Set(x => x.Content, content)
            .Set(x => x.ContentType, response.Content.Headers.ContentType?.MediaType)
            .Set(x => x.ETag, etag)
            .Set(x => x.LastModified, now.UtcDateTime)
            .Set(x => x.ExpiresAt, now.Add(effectiveTtl).UtcDateTime);

        await _cache.UpdateOneAsync(
            x => x.Api == _apiName && x.Endpoint == endpoint,
            cacheUpdate,
            new UpdateOptions { IsUpsert = true });

        return content;
    }

    public async Task<T> GetAsync<T>(string endpoint, TimeSpan? ttl = null, JsonSerializerOptions? jsonOptions = null) {
        var json = await GetStringAsync(endpoint, ttl);
        _logger.LogDebug("Deserializing response from {Endpoint} to type {Type}", endpoint, typeof(T).Name);
        return JsonSerializer.Deserialize<T>(json, jsonOptions)
               ?? throw new Exception($"Failed to deserialize response from {endpoint} to type {typeof(T).Name}");
    }
}

public sealed class TbaApiClient : ApiClientBase {
    public TbaApiClient(
        IHttpClientFactory httpClientFactory,
        IMongoCollection<CachedResponse> cache,
        ILogger<TbaApiClient> logger) : base(
        apiName: "TBA",
        httpClientName: "TBA",
        httpClientFactory: httpClientFactory,
        cache: cache,
        logger: logger) { }
}

public sealed class NexusApiClient : ApiClientBase {
    public NexusApiClient(
        IHttpClientFactory httpClientFactory,
        IMongoCollection<CachedResponse> cache,
        ILogger<NexusApiClient> logger) : base(
        apiName: "Nexus",
        httpClientName: "Nexus",
        httpClientFactory: httpClientFactory,
        cache: cache,
        logger: logger) { }
}