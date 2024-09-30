using ASPtask.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IFavoritesRepository _favoritesRepository;

        public FavoritesService(IFavoritesRepository favoritesRepository)
        {
            _favoritesRepository = favoritesRepository;
        }

        public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId)
        {
            return await _favoritesRepository.GetUserFavoritesAsync(userId);
        }

        public async Task<Favorite> AddFavoriteAsync(Favorite favorite)
        {
            //var favorite = new Favorite { ProductId = productId, UserId = userId };
            return await _favoritesRepository.AddFavoriteAsync(favorite);
        }

        public async Task<bool> RemoveFavoriteAsync(int favoriteId, int userId)
        {
            var favorite = await _favoritesRepository.GetFavoriteByIdAsync(favoriteId);
            if (favorite == null || favorite.UserId != userId) return false;

            return await _favoritesRepository.RemoveFavoriteAsync(favoriteId);
        }

        public async Task<bool> IsProductInFavoritesAsync(int productId, int userId)
        {
            return await _favoritesRepository.IsProductInFavoritesAsync(productId, userId);
        }

        public async Task<Favorite> GetFavoriteByIdAsync(int favoriteId)
        {
            return await _favoritesRepository.GetFavoriteByIdAsync(favoriteId);
        }

        public async Task<bool> UpdateFavoriteAsync(int favoriteId, Favorite updatedFavorite)
        {
            var existingFavorite = await _favoritesRepository.GetFavoriteByIdAsync(favoriteId);

            if (existingFavorite == null) return false;

            // Update properties
            existingFavorite.ProductId = updatedFavorite.ProductId;
            existingFavorite.UserId = updatedFavorite.UserId;

            return await _favoritesRepository.UpdateFavoriteAsync(existingFavorite);
        }
    }
}
