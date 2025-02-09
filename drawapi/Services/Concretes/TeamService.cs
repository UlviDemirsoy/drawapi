using drawapi.Data.Models;
using drawapi.Repositories.Abstracts;
using drawapi.Services.Abstracts;

namespace drawapi.Services.Concretes
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return await _teamRepository.GetTeamsAsync();
        }

        public async Task<Team> AddTeamAsync(Team team)
        {
            return await _teamRepository.AddTeamAsync(team);
        }
    }
}
