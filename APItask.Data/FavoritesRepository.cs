using APItask.Core.Models;
using APItask.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private readonly EssentialProductsDbContext _context;

        public FavoritesRepository(EssentialProductsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId)
        {
            return await _context.Favorites.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<Favorite?> GetFavoriteByIdAsync(int favoriteId)
        {
            return await _context.Favorites.FindAsync(favoriteId);
        }

        public async Task<Favorite> AddFavoriteAsync(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            return favorite ;
        }
        //public async Task<List<Product>> CreateProductsAsync(List<Product> products)
        //{
        //    await _context.Product.AddRangeAsync(products);
        //    await _context.SaveChangesAsync();
        //    return products;
        //}
        public async Task<bool> RemoveFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.Favorites.FindAsync(favoriteId);
            if (favorite == null) return false;

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsProductInFavoritesAsync(int productId, int userId)
        {
            return await _context.Favorites.AnyAsync(f => f.ProductId == productId && f.UserId == userId);
        }

        public async Task<bool> UpdateFavoriteAsync(Favorite favorite)
        {
            _context.Favorites.Update(favorite);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
