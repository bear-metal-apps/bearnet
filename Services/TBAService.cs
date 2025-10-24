using bearnet.Models.TBA;
using Microsoft.Extensions.Logging;

namespace bearnet.Services;

public class TBAService {
    private readonly TbaApiClient _api;
    public TBAService(
        TbaApiClient api,
        ILogger<TBAService> logger) {
        _api = api;
    }

    public Task<TBATeam> GetTeamAsync(string teamKey) =>
        _api.GetAsync<TBATeam>($"team/{teamKey}");

    public Task<TBAEvent> GetEventAsync(string eventKey) =>
        _api.GetAsync<TBAEvent>($"event/{eventKey}");

    public Task<TBAMatch> GetMatchAsync(string matchKey) =>
        _api.GetAsync<TBAMatch>($"match/{matchKey}");

    public Task<List<TBATeam>> GetEventTeamsAsync(string eventKey) =>
        _api.GetAsync<List<TBATeam>>($"event/{eventKey}/teams");

    public Task<List<TBAMatch>> GetEventMatchesAsync(string eventKey) =>
        _api.GetAsync<List<TBAMatch>>($"event/{eventKey}/matches");
}