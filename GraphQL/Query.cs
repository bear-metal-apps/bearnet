namespace bearnet.GraphQL;

using Models.TBA;
using Services;
using HotChocolate;

public class Query {
    /// <summary>
    /// Get a team by its key (e.g., "frc2046")
    /// </summary>
    public async Task<TBATeam> GetTeam(
        [Service] TBAService tbaService,
        string teamKey) {
        return await tbaService.GetTeamAsync(teamKey);
    }

    /// <summary>
    /// Get an event using its key (e.g., "2025wasno")
    /// </summary>
    public async Task<TBAEvent> GetEvent(
        [Service] TBAService tbaService,
        string eventKey) {
        return await tbaService.GetEventAsync(eventKey);
    }

    /// <summary>
    /// Get a match using its key (e.g., "2025wasno_qm1")
    /// </summary>
    public async Task<TBAMatch> GetMatch(
        [Service] TBAService tbaService,
        string matchKey) {
        return await tbaService.GetMatchAsync(matchKey);
    }

    /// <summary>
    /// Get all teams at an event using its key (e.g., "2025wasno")
    /// </summary>
    public async Task<List<TBATeam>> GetEventTeams(
        [Service] TBAService tbaService,
        string eventKey) {
        return await tbaService.GetEventTeamsAsync(eventKey);
    }

    /// <summary>
    /// Get all matches at an event using its key (e.g., "2025wasno")
    /// </summary>
    public async Task<List<TBAMatch>> GetEventMatches(
        [Service] TBAService tbaService,
        string eventKey) {
        return await tbaService.GetEventMatchesAsync(eventKey);
    }
}