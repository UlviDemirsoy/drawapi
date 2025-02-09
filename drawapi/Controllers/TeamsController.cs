using drawapi.Data.Models;
using drawapi.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace drawapi.Controllers
{
    [ApiController]
    [Route("api/teams")]
    public class TeamsController : Controller
    {

        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _teamService.GetTeamsAsync();
            return Ok(teams);
        }
        
    }
}
