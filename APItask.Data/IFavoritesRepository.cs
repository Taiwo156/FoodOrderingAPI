using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IFavoritesRepository
    {
<<<<<<< HEAD
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId);
        Task<Favorite?> GetFavoriteByIdAsync(int favoriteId);
=======
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId);
        Task<Favorite> GetFavoriteByIdAsync(int favoriteId);
>>>>>>> 6c79d9140c502456a00bc0950ae536f0f7d2003f
        Task<Favorite> AddFavoriteAsync(Favorite favorite);
        Task<bool> RemoveFavoriteAsync(int favoriteId);
        Task<bool> IsProductInFavoritesAsync(int productId, int userId);
        Task<bool> UpdateFavoriteAsync(Favorite favorite); 
    }
}
