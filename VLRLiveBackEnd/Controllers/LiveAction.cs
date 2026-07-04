using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VLRLiveBackEnd.Services;

namespace VLRLiveBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveAction : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly VLRapiService _service;

        //public LiveAction(IHttpClientFactory factory)
        //{
        //    _httpClient = factory.CreateClient();
        //}

        public LiveAction(VLRapiService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var response = await _httpClient.GetAsync("http://127.0.0.1:3001/match?q=live_score&num_pages=1&max_retries=3&request_delay=1&timeout=30");

        //    var json = await response.Content.ReadAsStringAsync();

        //    return Content(json, "application/json");
        //}
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetLiveMatchesAsync());
        }
        [HttpGet("{matchId}")]
        public async Task<IActionResult> GetMatch(string matchId)
        {
            var match = await _service.GetMatchDetailsAsync(matchId);

            return Ok(match);
        }
    }
}
