using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APItask.Core.Models;
using Microsoft.Extensions.Logging;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        ICategoryRepository categoryRepository,
        ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IReadOnlyCollection<Category>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return (IReadOnlyCollection<Category>)(categories?.ToList() ?? new List<Category>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all categories");
            throw new CategoryServiceException("Failed to retrieve categories", ex);
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Category ID must be positive", nameof(id));
        }

        try
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category with ID {CategoryId}", id);
            throw new CategoryServiceException($"Failed to retrieve category with ID {id}", ex);
        }
    }

    public async Task AddCategoryAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        ValidateCategory(category);

        try
        {
            await _categoryRepository.AddCategoryAsync(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new category");
            throw new CategoryServiceException("Failed to add category", ex);
        }
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        if (category.Id <= 0)
        {
            throw new ArgumentException("Invalid category ID", nameof(category));
        }

        ValidateCategory(category);

        try
        {
            await _categoryRepository.UpdateCategoryAsync(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category with ID {CategoryId}", category.Id);
            throw new CategoryServiceException($"Failed to update category with ID {category.Id}", ex);
        }
    }

    public async Task DeleteCategoryByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Category ID must be positive", nameof(id));
        }

        try
        {
            await _categoryRepository.DeleteCategoryByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category with ID {CategoryId}", id);
            throw new CategoryServiceException($"Failed to delete category with ID {id}", ex);
        }
    }

    private void ValidateCategory(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new ArgumentException("Category name is required", nameof(category));
        }

        // Add additional validation rules as needed
    }
}

public class CategoryServiceException : Exception
{
    public CategoryServiceException(string message) : base(message) { }
    public CategoryServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}