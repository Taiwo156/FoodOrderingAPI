using APItask.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public List<Product> Get()
        {
            return productService.GetProducts();
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post(List<Product> products)
        {
            await productService.CreateProduct(products);
            return Ok(new { message = "Products created successfully." });
        }

        // PUT api/<ProductController>/5
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

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await productService.DeleteProductAsync(id);
            if (result)
            {
                return Ok(new { message = "Product deleted successfully." });
            }
            return NotFound(new { message = "Product not found." });
        }
    }
}
