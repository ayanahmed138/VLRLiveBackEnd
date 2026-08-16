using Microsoft.Extensions.Hosting;
using VLRLiveBackEnd.Cache;
using VLRLiveBackEnd.Services;

namespace VLRLiveBackEnd.BackgroundServices;

public class LiveMatchPollingService : BackgroundService
{
    private readonly VLRapiService _service;
    private readonly ILogger<LiveMatchPollingService> _logger;
    private readonly LiveMatchCache _cache;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var matches = await _service.GetLiveMatchesAsync();

                foreach (var liveMatch in matches)
                {
                    var details = await _service.GetMatchDetailsAsync(liveMatch.MatchId);

                    _cache.Update(details);
                }

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
        ILogger<LiveMatchPollingService> logger)
    {
        _service = service;
        _cache = cache;
        _logger = logger;
    }
}