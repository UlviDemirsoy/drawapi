using drawapi.Data.Models;

namespace drawapi.Repositories.Abstracts
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAllGroupsAsync();
        Task<Group> GetGroupByIdAsync(int id);
        Task AddGroupAsync(Group group);
        Task AddTeamToGroupAsync(GroupTeam groupTeam);
        Task<List<Group>> GetAllGroupsByDrawIdAsync(int drawId);
    }
}
