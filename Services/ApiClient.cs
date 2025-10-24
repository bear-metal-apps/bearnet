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

        CreateIndexesAsync().GetAwaiter().GetResult();
    }

    private async Task CreateIndexesAsync() {
        try {
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
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Error creating cache indexes (may already exist)");
        }
    }

    public async Task<string> GetStringAsync(string endpoint, TimeSpan? ttl = null) {
        var now = DateTimeOffset.UtcNow;
        var cachedData = await _cache.Find(x => x.Api == _apiName && x.Endpoint == endpoint).FirstOrDefaultAsync();

        if (cachedData != null && cachedData.ExpiresAt > now) {
            return cachedData.Content;
        }

        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        if (!string.IsNullOrEmpty(cachedData?.ETag)) {
            request.Headers.TryAddWithoutValidation("If-None-Match", cachedData.ETag);
        }

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotModified && cachedData != null) {
            // Update cache expiration
            var filter = Builders<CachedResponse>.Filter.Where(x => x.Api == _apiName && x.Endpoint == endpoint);
            var update = Builders<CachedResponse>.Update
                .Set(x => x.ExpiresAt, now.Add(ttl ?? _defaultTtl))
                .Set(x => x.LastModified, now);
            await _cache.UpdateOneAsync(filter, update);
            return cachedData.Content;
        }

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var etag = response.Headers.ETag?.Tag;

        if (!string.IsNullOrEmpty(etag)) {
            var cacheUpdate = Builders<CachedResponse>.Update
                .Set(x => x.Api, _apiName)
                .Set(x => x.Endpoint, endpoint)
                .Set(x => x.Content, content)
                .Set(x => x.ContentType, response.Content.Headers.ContentType?.MediaType)
                .Set(x => x.ETag, etag)
                .Set(x => x.LastModified, now)
                .Set(x => x.ExpiresAt, now.Add(ttl ?? _defaultTtl));

            await _cache.UpdateOneAsync(
                x => x.Api == _apiName && x.Endpoint == endpoint,
                cacheUpdate,
                new UpdateOptions { IsUpsert = true });
        }

        return content;
    }

    public async Task<T> GetAsync<T>(string endpoint, TimeSpan? ttl = null, JsonSerializerOptions? jsonOptions = null) {
        var json = await GetStringAsync(endpoint, ttl);
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