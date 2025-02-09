using drawapi.Data.Models;
using drawapi.Data;
using drawapi.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace drawapi.Repositories.Concretes
{
    public class DrawRepository : IDrawRepository
    {
        private readonly AppDbContext _context;

        public DrawRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Draw>> ListAllDrawsAsync()
        {
            return await _context.Draws
                .Include(d => d.Groups)
                .ThenInclude(g => g.GroupTeams)
                .ThenInclude(gt => gt.Team)
                .ToListAsync();
        }


        public async Task<Draw> GetDrawByIdAsync(int id)
        {
            return await _context.Draws
                .Include(d => d.Groups)
                .ThenInclude(g => g.GroupTeams)
                .ThenInclude(gt => gt.Team)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task CreateDrawAsync(Draw draw)
        {
            _context.Draws.Add(draw); 
        }
    }
}
