namespace Bearnet;

using HotChocolate.AzureFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public class GraphQLFunction(IGraphQLRequestExecutor executor) {
    [Function("GraphQLHttpFunction")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.User, "get", "post", Route = "graphql/{**slug}")]
        HttpRequestData request)
        => executor.ExecuteAsync(request);
}