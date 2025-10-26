using System.Collections.Generic;
using System.Threading.Tasks;
using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace APItask.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly EssentialProductsDbContext _context;

        public ProductRepository(EssentialProductsDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            return await _context.Product.FindAsync(productId);
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Product.FindAsync(product.ProductId);
            if (existingProduct == null)
            {
                return null; // or throw an exception
            }

            existingProduct.Name = product.Name;
            existingProduct.Descriptions = product.Descriptions;
            existingProduct.Price = product.Price;
            existingProduct.AvailableSince = product.AvailableSince;
            existingProduct.CreatedDate = product.CreatedDate;
            existingProduct.CreatedBy = product.CreatedBy;
            existingProduct.ModifiedDate = product.ModifiedDate;
            existingProduct.ModifiedBy = product.ModifiedBy;
            existingProduct.IsActive = product.IsActive;

            _context.Product.Update(existingProduct);
            await _context.SaveChangesAsync();

            return existingProduct;
        }

        public async Task<List<Product>> CreateProductsAsync(List<Product> products)
        {
            await _context.Product.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            return products;
        }

        public async Task<bool> DeleteProductByIdAsync(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public Product? GetProduct(int productId)
        {
            return _context.Product.Find(productId);
        }

        public List<Product> GetProducts(int noOfProducts)
        {
            return _context.Product.Take(noOfProducts).ToList();
        }

        public Task<List<Product>> GetProductsAsync(int noOfProducts)
        {
            return _context.Product.Take(noOfProducts).ToListAsync();
        }
    }
}
