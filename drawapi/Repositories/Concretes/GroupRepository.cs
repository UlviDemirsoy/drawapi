using drawapi.Data;
using drawapi.Data.Models;
using drawapi.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace drawapi.Repositories.Concretes
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.GroupTeams)
                .ThenInclude(gt => gt.Team)
                .ToListAsync();
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.GroupTeams)
                .ThenInclude(gt => gt.Team)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddGroupAsync(Group group)
        {
            _context.Groups.Add(group);
        }

        public async Task AddTeamToGroupAsync(GroupTeam groupTeam)
        {
            _context.GroupTeams.Add(groupTeam);
        }

        public async Task<List<Group>> GetAllGroupsByDrawIdAsync(int drawId)
        {
            return await _context.Groups
                .Where(g => g.DrawId == drawId)
                .Include(g => g.GroupTeams)
                .ThenInclude(gt => gt.Team)
                .ToListAsync();
        }
    }
}
