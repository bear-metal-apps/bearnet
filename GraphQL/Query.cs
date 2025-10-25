namespace Bearnet.GraphQL;

using Bearnet.Models;
using Bearnet.Models.TBA;
using Bearnet.Services;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class Query {
    /// <summary>
    /// Get current authenticated user, creating if not exists
    /// </summary>
    public async Task<User> GetCurrentUser(
        [Service] UserService userService,
        [Service] IHttpContextAccessor httpContextAccessor) {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true) {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var userId = httpContext.User.FindFirst("oid")?.Value ??
                     httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) {
            throw new InvalidOperationException("User ID not found in claims");
        }

        var user = await userService.GetUserAsync(userId);
        if (user != null) {
            return user;
        }

        // Create new user from Entra claims
        var firstName = httpContext.User.FindFirst("given_name")?.Value ?? "";
        var lastName = httpContext.User.FindFirst("family_name")?.Value ?? "";
        var email = httpContext.User.FindFirst("email")?.Value ??
                    httpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "";

        var newUser = new User {
            Id = userId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Preferences = new UserPreferences {
                Theme = Theme.Light,
                ThemeColor = "#ffffff"
            },
            Roles = Array.Empty<Role>()
        };

        await userService.CreateUserAsync(newUser);
        return newUser;
    }
    /// <summary>
    /// Get a team by its key (e.g., "frc2046")
    /// </summary>
    public async Task<TBATeam> GetTeam(
        [Service] TbaApiClient api,
        string teamKey) {
        return await api.GetAsync<TBATeam>($"team/{teamKey}");
    }

    /// <summary>
    /// Get an event using its key (e.g., "2025wasno")
    /// </summary>
    public async Task<TBAEvent> GetEvent(
        [Service] TbaApiClient api,
        string eventKey) {
        return await api.GetAsync<TBAEvent>($"event/{eventKey}");
    }

    /// <summary>
    /// Get a match using its key (e.g., "2025wasno_qm1")
    /// </summary>
    public async Task<TBAMatch> GetMatch(
        [Service] TbaApiClient api,
        string matchKey) {
        return await api.GetAsync<TBAMatch>($"match/{matchKey}");
    }

    /// <summary>
    /// Get all teams at an event using its key (e.g., "2025wasno")
    /// </summary>
    public async Task<List<TBATeam>> GetEventTeams(
        [Service] TbaApiClient api,
        string eventKey) {
        return await api.GetAsync<List<TBATeam>>($"event/{eventKey}/teams");
    }

    /// <summary>
    /// Get all matches at an event using its key (e.g., "2025wasno")
    /// </summary>
    public async Task<List<TBAMatch>> GetEventMatches(
        [Service] TbaApiClient api,
        string eventKey) {
        return await api.GetAsync<List<TBAMatch>>($"event/{eventKey}/matches");
    }
}