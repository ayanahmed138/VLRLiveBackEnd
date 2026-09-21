using System.Collections.Concurrent;
using System.Threading.Channels;

namespace VLRLiveBackEnd.Services
{
    // One item on the to-do list: either we know the team id, or only the match id.
    public record TeamSyncJob(string? TeamId, string? MatchId);

    /// <summary>
    /// To-do list of teams whose logo we still need to download.
    /// Requests add to it and return immediately; TeamAutoSyncService works through it in the background.
    /// </summary>
    public class TeamSyncQueue
    {
        private static readonly TimeSpan RetryAfter = TimeSpan.FromMinutes(30);

        private readonly Channel<TeamSyncJob> _channel = Channel.CreateUnbounded<TeamSyncJob>();
        private readonly ConcurrentDictionary<string, DateTime> _lastQueued = new();

        public ChannelReader<TeamSyncJob> Reader => _channel.Reader;

        // Live matches: we know the team id
        public void EnqueueTeam(string teamId)
        {
            Enqueue($"team:{teamId}", new TeamSyncJob(teamId, null));
        }

        // Upcoming matches: we only know the match id, the worker finds the team ids from it
        public void EnqueueMatch(string matchId)
        {
            Enqueue($"match:{matchId}", new TeamSyncJob(null, matchId));
        }

        private void Enqueue(string key, TeamSyncJob job)
        {
            var now = DateTime.UtcNow;

            // Don't queue the same thing again for 30 minutes.
            // This also stops us hammering VLR if a team keeps failing.
            if (_lastQueued.TryGetValue(key, out var last) && now - last < RetryAfter)
                return;

            _lastQueued[key] = now;
            _channel.Writer.TryWrite(job);
        }
    }
}
