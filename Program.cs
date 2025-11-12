using Bearnet.GraphQL;
using Bearnet.Models;
using Bearnet.Models.TBA;
using Bearnet.Services;
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
        services.AddSingleton<IMongoDatabase>(sp => {
            var connectionString = config["COSMOS_CONNECTION_STRING"] ??
                                   throw new InvalidOperationException("COSMOS_CONNECTION_STRING is not configured.");
            var databaseName = config["COSMOS_DATABASE_NAME"] ??
                               throw new InvalidOperationException("COSMOS_DATABASE_NAME is not configured.");
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        });

        // Unified HTTP cache collection
        services.AddSingleton(sp => {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<CachedResponse>("http_cache");
        });

        // User collection
        services.AddSingleton<UserService>();

        services.AddHttpClient("TBA", client => {
            var baseUrl = "https://www.thebluealliance.com/api/v3/";
            var apiKey = config["TBA_API_KEY"] ??
                         throw new InvalidOperationException("TBA key is not configured.");

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("X-TBA-Auth-Key", apiKey);
        });
        services.AddHttpClient("Nexus", client => {
            var baseUrl = "https://frc.nexus/api/v1/";
            var apiKey = config["NEXUS_API_KEY"] ??
                         throw new InvalidOperationException("Nexus key is not configured.");
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Nexus-Api-Key", apiKey);
        });

        // API clients
        services.AddSingleton<TbaApiClient>();
        services.AddSingleton<NexusApiClient>();

        services.AddHttpContextAccessor();
    })
    .Build();

host.Run();

