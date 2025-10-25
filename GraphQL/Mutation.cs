namespace Bearnet.GraphQL;

using Bearnet.Models;
using Bearnet.Services;
using HotChocolate;

public class Mutation {
    /// <summary>
    /// Update an existing user
    /// </summary>
    public async Task<User> UpdateUser(
        [Service] UserService service,
        string id,
        User user) {
        user.Id = id;
        await service.UpdateUserAsync(id, user);
        return await service.GetUserAsync(id);
    }
}