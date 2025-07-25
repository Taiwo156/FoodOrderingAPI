﻿using Microsoft.AspNetCore.Mvc;
using APItask.Service;
using ASPtask.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using APItask;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    /// <summary>
    /// // GET: api/userId/{id}
    /// </summary> 
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<Favorite>>> GetUserFavorites(int userId)
    {
        var favorites = await _favoritesService.GetUserFavoritesAsync(userId);
        return Ok(favorites);
    }

    /// <summary>
    /// // POST: api/favorite
    /// </summary>
    /// <param name="favorite"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> AddFavorite(Favorite favorite)
    {
         await _favoritesService.AddFavoriteAsync(favorite);
        //return CreatedAtAction(nameof(GetUserFavorites), new { userId }, $"Favorite added successfully for product ID: {productId}");
        return Ok(new { message = "Favorite created successfully." });
    }


    /// <summary>
    /// // PUT: api/favorite/{id}
    /// </summary>
    /// <param name="favoriteId"></param>
    /// <param name="updatedFavorite"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPut("{favoriteId}")]
    public async Task<ActionResult<string>> UpdateFavorite(int favoriteId, [FromBody] Favorite updatedFavorite, [FromQuery] int userId)
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

    /// <summary>
    /// // DELETE: api/delivery/{id}
    /// </summary>
    /// <param name="favoriteId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{favoriteId}")]
    public async Task<ActionResult<string>> RemoveFavorite(int favoriteId, [FromQuery] int userId)
    {
        var result = await _favoritesService.RemoveFavoriteAsync(favoriteId, userId);
        if (!result) return NotFound("Favorite not found or not owned by the user.");
        return Ok("Favorite deleted successfully.");
    }

    /// <summary>
    /// // GET: api/productId/{id}
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("check/{productId}")]
    public async Task<ActionResult<string>> IsProductInFavorites(int productId, [FromQuery] int userId)
    {
        var isInFavorites = await _favoritesService.IsProductInFavoritesAsync(productId, userId);
        return Ok(isInFavorites ? "Product is in favorites." : "Product is not in favorites.");
    }

}
