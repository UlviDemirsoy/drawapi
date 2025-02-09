using drawapi.Data.Models;

namespace drawapi.Repositories.Abstracts
{
    public interface IDrawRepository
    {
        Task<List<Draw>> ListAllDrawsAsync();
        Task<Draw> GetDrawByIdAsync(int id);
        Task CreateDrawAsync(Draw draw);
    }
}
