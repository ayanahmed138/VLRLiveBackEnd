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
        public async Task<IActionResult> GetUpcoming()
        {
            return Ok(await _service.GetUpcomingMatchesAsync());
        }
    }
}
