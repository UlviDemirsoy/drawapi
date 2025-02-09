using drawapi.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace drawapi.Controllers
{
    [ApiController]
    [Route("api/draws")]
    public class DrawsController : ControllerBase
    {
        private readonly IDrawService _drawService;

        public DrawsController(IDrawService drawService)
        {
            _drawService = drawService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListAllDraws()
        {
            var draws = await _drawService.ListAllDrawsAsync();
            return Ok(draws);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrawById(int id)
        {
            var draw = await _drawService.GetDrawByIdAsync(id);
            return draw != null ? Ok(draw) : NotFound(new { message = "Draw not found" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDraw([FromQuery] string drawerName, [FromQuery] int groupCount)
        {
            if (string.IsNullOrEmpty(drawerName))
                return BadRequest(new { message = "Drawer name is required." });

            if (groupCount != 4 && groupCount != 8)
                return BadRequest(new { message = "Group count must be 4 or 8." });

            var draw = await _drawService.CreateDrawAsync(drawerName, groupCount);
            return CreatedAtAction(nameof(GetDrawById), new { id = draw.Id }, draw);
        }

    }
}
