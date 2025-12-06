using APItask.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APItask.Core.Models;

namespace APItask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductByStoreController : ControllerBase
    {
        private readonly IProductByStoreService _service;

        public ProductByStoreController(IProductByStoreService service)
        {
            _service = service;
        }

        /// <summary>
        /// ProductByStore API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProductsInStore()
        {
            var products = await _service.GetProductsInStore();
            return Ok(products);
        }

        /// <summary>
        /// // GET: api/productId/{id}/storeId/{id}
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet("{productId}/{storeId}")]
        public async Task<IActionResult> GetProductById(int productId, int storeId)
        {
            var product = await _service.GetProductById(productId, storeId);
            return product == null ? NotFound() : Ok(product);
        }

        /// <summary>
        /// // POST: api/ProductByStore
        /// </summary>
        /// <param name="productByStore"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddProductToStore([FromBody] ProductByStore productByStore)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddProductToStore(productByStore);
            return Ok(new { message = "Product registered successfully in store." });
        }

        /// <summary>
        /// PUT: api/Products/{id}
        /// </summary>
        /// <param name="productByStore"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProductInStore([FromBody] ProductByStore productByStore)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateProductInStore(productByStore);
            return Ok(new { message = "Product updated successfully in store." });
        }

        /// <summary>
        /// // DELETE: api/ProductsByStore/{id}
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpDelete("{productId}/{storeId}")]
        public async Task<IActionResult> DeleteProductFromStore(int productId, int storeId)
        {
            await _service.DeleteProductFromStore(productId, storeId);
            return Ok(new { message = "Product deleted successfully from store." });
        }
    }
}
