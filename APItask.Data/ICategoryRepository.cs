using System.Collections.Generic;
using System.Threading.Tasks;
using APItask.Core.Models;
using APItask.Data; // Adjust this based on your actual models

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<List<Category>> AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryByIdAsync(int id);
}
