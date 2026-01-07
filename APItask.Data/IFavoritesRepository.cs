using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IFavoritesRepository
    {
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId);
        Task<Favorite?> GetFavoriteByIdAsync(int favoriteId);

        Task<Favorite> AddFavoriteAsync(Favorite favorite);
        Task<bool> RemoveFavoriteAsync(int favoriteId);
        Task<bool> IsProductInFavoritesAsync(int productId, int userId);
        Task<bool> UpdateFavoriteAsync(Favorite favorite); 
    }
}
