using System.Collections.Generic;
using System.Threading.Tasks;
using APItask.Core.Models;

public interface ICategoryService
{
    Task<IReadOnlyCollection<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryByIdAsync(int id);
}