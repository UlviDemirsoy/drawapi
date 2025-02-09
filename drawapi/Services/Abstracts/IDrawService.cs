using drawapi.Data.Dtos;
using drawapi.Data.Models;

namespace drawapi.Services.Abstracts
{
    public interface IDrawService
    {
        Task<List<DrawDTO>> ListAllDrawsAsync();
        Task<DrawDTO> GetDrawByIdAsync(int id);
        Task<DrawDTO> CreateDrawAsync(string drawerName, int groupCount);
    }
}
