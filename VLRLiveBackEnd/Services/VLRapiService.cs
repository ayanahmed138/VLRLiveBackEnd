using System.Text.Json;
using VLRLiveBackEnd.DTOs;
using VLRLiveBackEnd.Models;
using VLRLiveBackEnd.Models.Event;
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
            var mapNumber = match.maps?.Length ?? 0;

            return new LiveMatchDetailsDto
            {
                MatchId = match.match_id,
                Event = match._event?.name ?? "",
                Team1 = match.teams[0].name,
                Team2 = match.teams[1].name,
                Team1Id = match.teams[0].id,
                Team2Id = match.teams[1].id,
                Team1Logo = match.teams[0].logo,
                Team2Logo = match.teams[1].logo,
                SeriesScore = $"{match.teams[0].score}-{match.teams[1].score}",
                CurrentMap = currentMap?.map_name ?? "Not Started",
                CurrentMapScore = currentMap == null
        ? "0-0"
        : $"{currentMap.score.team1}-{currentMap.score.team2}",
                MapNumber = mapNumber
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

        public async Task<List<VLRTeamDto>> GetTeamsFromEventAsync(string eventId)
        {
            var response = await _httpClient.GetAsync($"/v2/event/{eventId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<EventResponse>(json);

            if (result?.data?.segments?.teams == null)
            {
                throw new Exception("No teams found for this event.");
            }

            return result.data.segments.teams
                .Select(team => new VLRTeamDto
                {
                    Id = team.id,
                    Name = team.name
                })
                .ToList();
        }
        public async Task<VLRTeamDto> GetTeamAsync(string teamId)
        {
            var response = await _httpClient.GetAsync(
                $"/v2/team?id={teamId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TeamResponse>(json);

            if (result?.data?.segments == null ||
                result.data.segments.Length == 0)
            {
                throw new Exception($"Team {teamId} not found.");
            }

            var team = result.data.segments[0];

            return new VLRTeamDto
            {
                Id = team.id,
                Name = team.name,
                Tag = team.tag,
                Country = team.country,
                Region = team.country, // we'll fix region later
                Logo = team.logo
            };
        }
        // Only pulls the team ids out of a match (works for upcoming matches too)
        public async Task<List<string>> GetMatchTeamIdsAsync(string matchId)
        {
            var response = await _httpClient.GetAsync($"/v2/match/details?match_id={matchId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var ids = new List<string>();

            if (doc.RootElement.TryGetProperty("data", out var data)
                && data.TryGetProperty("segments", out var segments)
                && segments.ValueKind == JsonValueKind.Array
                && segments.GetArrayLength() > 0
                && segments[0].TryGetProperty("teams", out var teams)
                && teams.ValueKind == JsonValueKind.Array)
            {
                foreach (var team in teams.EnumerateArray())
                {
                    if (!team.TryGetProperty("id", out var idElement))
                        continue;

                    var id = idElement.ValueKind == JsonValueKind.String
                        ? idElement.GetString()
                        : idElement.ToString();

                    if (!string.IsNullOrWhiteSpace(id))
                        ids.Add(id);
                }
            }

            return ids;
        }

        public async Task<string> GetTeamRawAsync(string teamId)
        {
            var response = await _httpClient.GetAsync(
                $"/v2/team?id={teamId}&q=profile");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
