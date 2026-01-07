using APItask.Core.Models;
using APItask.Service;
﻿using APItask.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<Product> products)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productService.CreateProduct(products);
            return Ok(new { message = "Products created successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
                return BadRequest(new { message = "Product ID mismatch." });

            var existingProduct = await _productService.GetProductAsync(id);
            if (existingProduct == null)
                return NotFound(new { message = "Product not found." });

            await _productService.UpdateProductAsync(product);
            return Ok(new { message = "Product updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound("Product not found.");
            }
            return Ok("Product deleted successfully.");
        }

        
    }
}
