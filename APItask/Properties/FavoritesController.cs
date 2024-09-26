using Microsoft.AspNetCore.Mvc;
using APItask.Service;
using ASPtask.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<Favorite>>> GetUserFavorites(string userId)
    {
        var favorites = await _favoritesService.GetUserFavoritesAsync(userId);
        return Ok(favorites);
    }

    [HttpPost]
    public async Task<ActionResult<string>> AddFavorite([FromBody] int productId, [FromQuery] string userId)
    {
        var favorite = await _favoritesService.AddFavoriteAsync(productId, userId);
        return CreatedAtAction(nameof(GetUserFavorites), new { userId }, $"Favorite added successfully for product ID: {productId}");
    }

    [HttpPut("{favoriteId}")]
    public async Task<ActionResult<string>> UpdateFavorite(int favoriteId, [FromBody] Favorite updatedFavorite, [FromQuery] string userId)
    {
        // Check if the favorite belongs to the user
        var existingFavorite = await _favoritesService.GetFavoriteByIdAsync(favoriteId);

        if (existingFavorite == null)
        {
            return NotFound("Favorite not found.");
        }

        if (existingFavorite.UserId != userId)
        {
            return Forbid("You can only update your own favorites.");
        }

        // Update the favorite (ProductId and UserId)
        var result = await _favoritesService.UpdateFavoriteAsync(favoriteId, updatedFavorite);

        if (!result)
        {
            return StatusCode(500, "Error updating favorite.");
        }

        return Ok("Favorite updated successfully.");
    }

    [HttpDelete("{favoriteId}")]
    public async Task<ActionResult<string>> RemoveFavorite(int favoriteId, [FromQuery] string userId)
    {
        var result = await _favoritesService.RemoveFavoriteAsync(favoriteId, userId);
        if (!result) return NotFound("Favorite not found or not owned by the user.");
        return Ok("Favorite deleted successfully.");
    }

    [HttpGet("check/{productId}")]
    public async Task<ActionResult<string>> IsProductInFavorites(int productId, [FromQuery] string userId)
    {
        var isInFavorites = await _favoritesService.IsProductInFavoritesAsync(productId, userId);
        return Ok(isInFavorites ? "Product is in favorites." : "Product is not in favorites.");
    }

}
