using drawapi.Data.Models;
using drawapi.Data;
using drawapi.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace drawapi.Repositories.Concretes
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> AddTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }
    }
}
