using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VLRLiveBackEnd.Cache;
using VLRLiveBackEnd.Services;

namespace VLRLiveBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveAction : ControllerBase
    {
        private readonly LiveMatchCache _cache;
        private readonly VLRapiService _service;


        public LiveAction(LiveMatchCache cache, VLRapiService service)
        {
            _cache = cache;
            _service = service;
        }





        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_cache.GetAll());
        }
        [HttpGet("{matchId}")]
        public IActionResult GetMatch(string matchId)
        {
            var match = _cache.Get(matchId);

            if (match == null)
                return NotFound();

            return Ok(match);
        }
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming(
            [FromServices] TeamIconResolver iconResolver,
            [FromServices] TeamSyncQueue syncQueue)
        {
            var matches = await _service.GetUpcomingMatchesAsync();
            var icons = await iconResolver.GetIconsByNameAsync();

            foreach (var match in matches)
            {
                match.Team1Logo = icons.GetValueOrDefault(match.Team1 ?? "");
                match.Team2Logo = icons.GetValueOrDefault(match.Team2 ?? "");

                // A logo is missing: ask the background worker to fetch it.
                // This only adds to a list, so it does not slow this request down.
                if (match.Team1Logo == null || match.Team2Logo == null)
                {
                    var matchId = Regex.Match(match.MatchPage ?? "", @"\d+").Value;

                    if (matchId != "")
                        syncQueue.EnqueueMatch(matchId);
                }
            }

            return Ok(matches);
        }
        [HttpGet("event/{eventId}/teams")]
        public async Task<IActionResult> GetEventTeams(string eventId)
        {
            var teams = await _service.GetTeamsFromEventAsync(eventId);

            return Ok(teams);
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetTeam(string teamId)
        {
            var team = await _service.GetTeamAsync(teamId);

            return Ok(team);
        }

        [HttpGet("team/{teamId}/raw")]
        public async Task<IActionResult> GetTeamRaw(string teamId)
        {
            var result = await _service.GetTeamRawAsync(teamId);

            return Content(result, "application/json");
        }
        [HttpPost("sync-team/{teamId}")]
        public async Task<IActionResult> SyncTeam(string teamId, [FromServices] TeamSyncService teamSyncService)
        {
            await teamSyncService.SyncTeamAsync(teamId);

            return Ok(new
            {
                message = "Team synced successfully",
                teamId
            });
        }

        [HttpPost("sync-event/{eventId}")]
        public async Task<IActionResult> SyncEvent(string eventId, [FromServices] TeamSyncService teamSyncService)
        {
            await teamSyncService.SyncTeamsFromEventAsync(eventId);

            return Ok(new
            {
                message = "Event teams synced successfully",
                eventId
            });
        }

        [HttpPost("team/{teamId}/generate-icon")]
        public async Task<IActionResult> GenerateTeamIcon(
    string teamId,
    [FromServices] TeamLogoService logoService)
        {
            await logoService.GenerateTeamIconAsync(teamId);

            return Ok(new
            {
                message = "Team icon generated successfully.",
                teamId
            });
        }
    }
}


