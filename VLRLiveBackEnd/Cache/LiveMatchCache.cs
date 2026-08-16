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

    public List<LiveMatchDetailsDto> GetAll()
    {
        return _matches.Values.ToList();
    }
}