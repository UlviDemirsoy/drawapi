using drawapi.Data.Models;

namespace drawapi.Services.Abstracts
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        Task<Team> AddTeamAsync(Team team);
    }
}
