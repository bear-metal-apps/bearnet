namespace Bearnet.GraphQL;

using Bearnet.Models;
using Bearnet.Models.TBA;
using Bearnet.Services;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class Query {
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