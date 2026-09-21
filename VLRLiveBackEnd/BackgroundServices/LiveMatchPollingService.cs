using Microsoft.Extensions.Hosting;
using VLRLiveBackEnd.Cache;
using VLRLiveBackEnd.Services;

namespace VLRLiveBackEnd.BackgroundServices;

public class LiveMatchPollingService : BackgroundService
{
    private readonly VLRapiService _service;
    private readonly ILogger<LiveMatchPollingService> _logger;
    private readonly LiveMatchCache _cache;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TeamSyncQueue _syncQueue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var matches = await _service.GetLiveMatchesAsync();

                // A background service lives forever, but the DbContext must not,
                // so we open a short-lived scope on every poll.
                using var scope = _scopeFactory.CreateScope();
                var iconResolver = scope.ServiceProvider.GetRequiredService<TeamIconResolver>();
                var icons = await iconResolver.GetIconsByIdAsync();

                var liveIds = new List<string>();

                foreach (var liveMatch in matches)
                {
                    var details = await _service.GetMatchDetailsAsync(liveMatch.MatchId);

                    // Prefer our own hosted icon, otherwise keep VLR's logo URL
                    // and ask the background worker to download the icon for next time
                    if (details.Team1Id != null)
                    {
                        if (icons.TryGetValue(details.Team1Id, out var icon1))
                            details.Team1Logo = icon1;
                        else
                            _syncQueue.EnqueueTeam(details.Team1Id);
                    }

                    if (details.Team2Id != null)
                    {
                        if (icons.TryGetValue(details.Team2Id, out var icon2))
                            details.Team2Logo = icon2;
                        else
                            _syncQueue.EnqueueTeam(details.Team2Id);
                    }

                    _cache.Update(details);
                    liveIds.Add(details.MatchId);
                }

                // Matches that finished since the last poll disappear from the cache
                _cache.RemoveAllExcept(liveIds);

                _logger.LogInformation("Cached {Count} live matches", matches.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while polling VLR API");
            }

            await Task.Delay(30000, stoppingToken);
        }
    }

    public LiveMatchPollingService(
        VLRapiService service,
        LiveMatchCache cache,
        IServiceScopeFactory scopeFactory,
        TeamSyncQueue syncQueue,
        ILogger<LiveMatchPollingService> logger)
    {
        _service = service;
        _cache = cache;
        _scopeFactory = scopeFactory;
        _syncQueue = syncQueue;
        _logger = logger;
    }
}
