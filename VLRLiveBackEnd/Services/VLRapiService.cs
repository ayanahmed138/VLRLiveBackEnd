using System.Text.Json;
using VLRLiveBackEnd.DTOs;
using VLRLiveBackEnd.Models;
using VLRLiveBackEnd.Models.MatchDetails;

namespace VLRLiveBackEnd.Services
{
    public class VLRapiService
    {
        private readonly HttpClient _httpClient;
       


        public VLRapiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<LiveMatchDto>> GetLiveMatchesAsync()
        {
            var response = await _httpClient.GetAsync("/match?q=live_score&num_pages=1&max_retries=3&request_delay=1&timeout=30");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<LiveResponse>(json);
            var matches = result.data.segments.Select(match => new LiveMatchDto
            {
                MatchId = match.match_id,
                Team1 = match.team1,
                Team2 = match.team2,
                Score = $"{match.score1}-{match.score2}",
                Event = match.match_event,
                Status = match.time_until_match
            }).ToList();

            return matches;

            
        }
        public async Task<MatchDetailsResponse> GetMatchDetailsAsync(string matchId)
        {
            var response = await _httpClient.GetAsync($"/v2/match/details?match_id={matchId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<MatchDetailsResponse>(json)!;
        }

    }
}
