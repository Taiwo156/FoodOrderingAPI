using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IFavoritesService
    {
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId);
        Task<Favorite> AddFavoriteAsync(int productId, string userId);
        Task<bool> RemoveFavoriteAsync(int favoriteId, string userId);
        Task<bool> IsProductInFavoritesAsync(int productId, string userId);
        Task<Favorite> GetFavoriteByIdAsync(int favoriteId); 
        Task<bool> UpdateFavoriteAsync(int favoriteId, Favorite updatedFavorite); 
    }
}
