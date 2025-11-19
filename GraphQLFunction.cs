namespace Bearnet;

using System.Threading.Tasks;
using System.Net;
using HotChocolate.AzureFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public class GraphQLFunction(IGraphQLRequestExecutor executor)
{
    [Function("GraphQLHttpFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "graphql/{**slug}")]
        HttpRequestData request)
    {
        var apiKeySecret = Environment.GetEnvironmentVariable("API_KEY_SECRET");

        // Read header (e.g. X-Api-Key)
        if (!request.Headers.TryGetValues("X-Api-Key", out var values) ||
            string.IsNullOrWhiteSpace(apiKeySecret) ||
            values.FirstOrDefault() != apiKeySecret)
        {
            var unauthorized = request.CreateResponse(HttpStatusCode.Unauthorized);
            await unauthorized.WriteStringAsync("Unauthorized");
            return unauthorized;
        }

        // If valid, proceed to GraphQL executor
        return await executor.ExecuteAsync(request);
    }
}
