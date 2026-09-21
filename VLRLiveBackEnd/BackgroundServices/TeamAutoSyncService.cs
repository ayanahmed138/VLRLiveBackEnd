using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using VLRLiveBackEnd.Data;
using VLRLiveBackEnd.Services;

namespace VLRLiveBackEnd.BackgroundServices;

/// <summary>
/// Quietly downloads logos for teams we haven't seen before.
/// Works through the queue one team at a time so it never slows down API requests.
/// </summary>
public class TeamAutoSyncService : BackgroundService
{
    private readonly TeamSyncQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TeamAutoSyncService> _logger;

    public TeamAutoSyncService(
        TeamSyncQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<TeamAutoSyncService> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var job in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await HandleAsync(job, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Auto-sync failed for {Job}", job);
            }
        }
    }

    private async Task HandleAsync(TeamSyncJob job, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var vlr = scope.ServiceProvider.GetRequiredService<VLRapiService>();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var sync = scope.ServiceProvider.GetRequiredService<TeamSyncService>();

        var teamIds = new List<string>();

        if (job.TeamId != null)
            teamIds.Add(job.TeamId);

        if (job.MatchId != null)
            teamIds.AddRange(await vlr.GetMatchTeamIdsAsync(job.MatchId));

        foreach (var teamId in teamIds)
        {
            try
            {
                // Already have its icon? Nothing to do.
                var alreadyDone = await db.Teams
                    .AnyAsync(t => t.VlrTeamId == teamId && t.IconPath != null, ct);

                if (alreadyDone)
                    continue;

                await sync.SyncTeamAsync(teamId);
                _logger.LogInformation("Auto-synced team {TeamId}", teamId);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Could not sync team {TeamId}", teamId);
            }

            // Be gentle with the VLR API
            await Task.Delay(1000, ct);
        }
    }
}
