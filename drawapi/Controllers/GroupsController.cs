using drawapi.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace drawapi.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Get all groups with their teams.
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            return group != null ? Ok(group) : NotFound(new { message = "Group not found" });
        }

    }
}
