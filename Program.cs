using Bearnet.GraphQL;
using Bearnet.Models;
using Bearnet.Models.TBA;
using Bearnet.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Bearnet.Models.Cache;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .AddGraphQLFunction(b => b
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddType<User>()
        .AddType<UserPreferences>()
        .AddType<Theme>()
        .AddType<Role>()
        .AddType<IScoreBreakdown>()
        .AddType<ScoreBreakdown2015>()
        .AddType<ScoreBreakdown2025>()
        .AddType<AllianceScoreBreakdown2015>()
        .AddType<AllianceScoreBreakdown2025>()
        .AddMongoDbProjections()
        .AddMongoDbFiltering()
        .AddMongoDbSorting()
        .AddMongoDbPagingProviders())
    .ConfigureServices((context, services) => {
        var config = context.Configuration;

        // MongoDB/CosmosDB Setup
        services.AddSingleton<IMongoClient>(sp => {
            var connectionString = config["COSMOS_CONNECTION_STRING"] ??
                                   throw new InvalidOperationException("COSMOS_CONNECTION_STRING is not configured.");

            var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);
            mongoSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
            mongoSettings.RetryReads = true;
            mongoSettings.RetryWrites = false; // Cosmos DB requires retryWrites=false
            mongoSettings.ApplicationName = "Bearnet";

            return new MongoClient(mongoSettings);
        });

        services.AddSingleton<IMongoDatabase>(sp => {
            var databaseName = config["COSMOS_DATABASE_NAME"] ??
                               throw new InvalidOperationException("COSMOS_DATABASE_NAME is not configured.");
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        // Unified HTTP cache collection
        services.AddSingleton(sp => {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<CachedResponse>("http_cache");
        });

        // User collection
        services.AddSingleton<UserService>();

        RegisterExternalApi(services, config, "TBA", "TBA_API_KEY", "https://www.thebluealliance.com/api/v3/", "X-TBA-Auth-Key");
        RegisterExternalApi(services, config, "Nexus", "NEXUS_API_KEY", "https://frc.nexus/api/v1/", "Nexus-Api-Key");

        // API clients
        services.AddSingleton<TbaApiClient>();
        services.AddSingleton<NexusApiClient>();

        services.AddHttpContextAccessor();
    })
    .Build();

host.Run();

static void RegisterExternalApi(IServiceCollection services, IConfiguration config, string clientName, string apiKeySetting, string defaultBaseUrl, string headerName) {
    services.AddHttpClient(clientName, (sp, client) => {
        var apiKey = config[apiKeySetting] ?? throw new InvalidOperationException($"{apiKeySetting} is not configured.");
        var configuredBaseUrl = config[$"{clientName.ToUpperInvariant()}_BASE_URL"] ?? defaultBaseUrl;

        if (!Uri.TryCreate(configuredBaseUrl, UriKind.Absolute, out var baseAddress)) {
            throw new InvalidOperationException($"Invalid base URL for {clientName}: {configuredBaseUrl}");
        }

        client.BaseAddress = baseAddress;
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Remove(headerName);
        client.DefaultRequestHeaders.Add(headerName, apiKey);
    });
}

