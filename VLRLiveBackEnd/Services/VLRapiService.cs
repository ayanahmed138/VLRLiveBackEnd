using System.Text.Json;
using VLRLiveBackEnd.DTOs;
using VLRLiveBackEnd.Models;
using VLRLiveBackEnd.Models.MatchDetails;
using VLRLiveBackEnd.Models.Upcoming;

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
        public async Task<LiveMatchDetailsDto> GetMatchDetailsAsync(string matchId)
        {
            var response = await _httpClient.GetAsync($"/v2/match/details?match_id={matchId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<MatchDetailsResponse>(json)!;
            if (result.data?.segments == null || result.data.segments.Length == 0)
            {
                throw new Exception("Match not found.");
            }

            var match = result.data.segments[0];
            if (match.teams.Length < 2)
            {
                
                throw new Exception("Invalid match data.");
                
            }

            var currentMap = match.maps.LastOrDefault();

            return new LiveMatchDetailsDto
            {
                MatchId = match.match_id,
                Event = match._event?.name ?? "",
                Team1 = match.teams[0].name,
                Team2 = match.teams[1].name,
                Team1Logo = match.teams[0].logo,
                Team2Logo = match.teams[1].logo,
                SeriesScore = $"{match.teams[0].score}-{match.teams[1].score}",
                CurrentMap = currentMap?.map_name ?? "Not Started",
                CurrentMapScore = currentMap == null
        ? "0-0"
        : $"{currentMap.score.team1}-{currentMap.score.team2}"
            };
        }

        public async Task<List<UpcomingMatchDto>> GetUpcomingMatchesAsync()
        {
            var response = await _httpClient.GetAsync(
                "/v2/match?q=upcoming&num_pages=1&max_retries=3&request_delay=1&timeout=30");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<UpcomingResponse>(json)!;

            return result.data.segments.Select(match => new UpcomingMatchDto
            {
                Team1 = match.team1,
                Team2 = match.team2,
                Event = match.match_event,
                Series = match.match_series,
                StartsIn = match.time_until_match,
                MatchPage = match.match_page,
                UnixTimestamp = match.unix_timestamp
            }).ToList();
        }


    }
}
