using ASPtask.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IFavoritesService
    {
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId);
        Task<Favorite> AddFavoriteAsync(Favorite favorite);
        Task<bool> RemoveFavoriteAsync(int favoriteId, int userId);
        Task<bool> IsProductInFavoritesAsync(int productId, int userId);
        Task<Favorite> GetFavoriteByIdAsync(int favoriteId); 
        Task<bool> UpdateFavoriteAsync(int favoriteId, Favorite updatedFavorite); 
    }
}
