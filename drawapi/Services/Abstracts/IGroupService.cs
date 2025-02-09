using drawapi.Data.Dtos;
using drawapi.Data.Models;

namespace drawapi.Services.Abstracts
{
    public interface IGroupService
    {
        Task<List<GroupDTO>> GetAllGroupsAsync();
        Task<GroupDTO> GetGroupByIdAsync(int id);
        Task<Group> CreateGroupAsync(Group group);
        Task AddTeamToGroupAsync(GroupTeam groupTeam);
        Task<List<Group>> GetAllGroupsByDrawIdAsync(int drawId);
    }
}
