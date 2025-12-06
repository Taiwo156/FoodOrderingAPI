<<<<<<< HEAD
﻿using APItask.Core.Models;
using APItask.Service;
=======
﻿using APItask.Service;
using ASPtask.Core;
>>>>>>> 6c79d9140c502456a00bc0950ae536f0f7d2003f
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace APItask.Properties
=======
namespace APItask.Controllers
>>>>>>> 6c79d9140c502456a00bc0950ae536f0f7d2003f
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
<<<<<<< HEAD
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
            if (result)
                return Ok(new { message = "Product deleted successfully." });

=======
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// // GET: api/<ProductController>
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public List<Product> Get()
        {
            return productService.GetProducts();
        }

        /// <summary>
        /// // GET: api/product/{productId}
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductAsync(int productId)
        {
            var product = await productService.GetProductAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        /// <summary>
        /// // GET: api/product/{productId}
        /// </summary>
        /// <param name="upc"></param>
        /// <returns></returns>
        [HttpGet("upc")]
        public async Task<ActionResult<Product>> GetUpcAsync(string upc)
        {
            var product = await productService.GetUpcAsync(upc);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        /// <summary>
        /// // POST api/<ProductController>
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(List<Product> products)
        {
            await productService.CreateProduct(products);
            return Ok(new { message = "Products created successfully." });
        }

        /// <summary>
        /// // PUT api/<ProductController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest(new { message = "Product ID mismatch." });
            }

            var existingProduct = await productService.GetProductAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            // Update the properties of the existing product
            existingProduct.Name = product.Name;
            existingProduct.Descriptions = product.Descriptions;
            existingProduct.Price = product.Price;
            existingProduct.AvailableSince = product.AvailableSince;
            existingProduct.CreatedDate = product.CreatedDate;
            existingProduct.CreatedBy = product.CreatedBy;
            existingProduct.ModifiedDate = product.ModifiedDate;
            existingProduct.ModifiedBy = product.ModifiedBy;
            existingProduct.IsActive = product.IsActive;

            await productService.UpdateProductAsync(existingProduct);

            return Ok(new { message = "Product updated successfully." });
        }

        /// <summary>
        /// // DELETE api/<ProductController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await productService.DeleteProductAsync(id);
            if (result)
            {
                return Ok(new { message = "Product deleted successfully." });
            }
>>>>>>> 6c79d9140c502456a00bc0950ae536f0f7d2003f
            return NotFound(new { message = "Product not found." });
        }
    }
}
