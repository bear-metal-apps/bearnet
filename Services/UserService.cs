using System.Collections.Generic;
using Bearnet.Models;
using MongoDB.Driver;

namespace Bearnet.Services;

public class UserService {
    private readonly IMongoCollection<User> _users;

    public UserService(IMongoDatabase database) {
        _users = database.GetCollection<User>("users");
    }

    public async Task<User> GetUserAsync(string id) {
        var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        return user ?? throw new KeyNotFoundException($"User with id '{id}' was not found.");
    }

    public async Task CreateUserAsync(User user) {
        await _users.InsertOneAsync(user);
    }

    public async Task UpdateUserAsync(string id, User user) {
        var result = await _users.ReplaceOneAsync(u => u.Id == id, user);
        if (result.MatchedCount == 0) {
            throw new KeyNotFoundException($"User with id '{id}' was not found.");
        }
    }

    public async Task DeleteUserAsync(string id) {
        await _users.DeleteOneAsync(u => u.Id == id);
    }
}