using System.Collections.Concurrent;
using VLRLiveBackEnd.DTOs;

namespace VLRLiveBackEnd.Cache;

public class LiveMatchCache
{
    private readonly ConcurrentDictionary<string, LiveMatchDetailsDto> _matches = new();

    public void Update(LiveMatchDetailsDto match)
    {
        _matches[match.MatchId] = match;
    }

    public LiveMatchDetailsDto? Get(string matchId)
    {
        _matches.TryGetValue(matchId, out var match);
        return match;
    }

    // Drop matches that are no longer live (finished matches would otherwise stay forever)
    public void RemoveAllExcept(IEnumerable<string> liveMatchIds)
    {
        var keep = new HashSet<string>(liveMatchIds);

        foreach (var id in _matches.Keys)
        {
            if (!keep.Contains(id))
                _matches.TryRemove(id, out _);
        }
    }

    public List<LiveMatchDetailsDto> GetAll()
    {
        return _matches.Values.ToList();
    }
}