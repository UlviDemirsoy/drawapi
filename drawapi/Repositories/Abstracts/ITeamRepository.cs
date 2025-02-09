using drawapi.Data.Models;

namespace drawapi.Repositories.Abstracts
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        Task<Team> AddTeamAsync(Team team);
    }
}
