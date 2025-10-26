using System.Collections.Generic;
using System.Threading.Tasks;
using APItask.Core.Models;
using APItask.Data;
using Microsoft.EntityFrameworkCore;


public class CategoryRepository : ICategoryRepository
{
    private readonly EssentialProductsDbContext _context;

    public CategoryRepository(EssentialProductsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Category.ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Category.FindAsync(id);
    }

    public async Task<List<Category>> AddCategoryAsync(Category category)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync();
        return await _context.Category.ToListAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Category.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryByIdAsync(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category != null)
        {
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
