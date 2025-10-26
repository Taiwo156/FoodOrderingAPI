using APItask.Data;
using APItask.Service;
using Microsoft.AspNetCore.Mvc;
using APItask.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItask.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // GET: api/store
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetAllStores()
        {
            var stores = await _storeService.GetAllStoresAsync();
            return Ok(stores);
        }

        // GET: api/store/{storeId}
        [HttpGet("{storeId:int}")]
        public async Task<ActionResult<Store>> GetStoreById(int storeId)
        {
            var store = await _storeService.GetStoreByIdAsync(storeId);
            if (store == null)
            {
                return NotFound("Store not found.");
            }
            return Ok(store);
        }

        // POST: api/store
        [HttpPost]
        public async Task<ActionResult<Store>> AddStore([FromBody] Store store)
        {
            if (store == null)
            {
                return BadRequest("Store cannot be null.");
            }

            var createdStore = await _storeService.AddStoreAsync(store);
            return Ok(new { message = "Stores created successfully." });

        }

        // PUT: api/store
        [HttpPut]
        public async Task<ActionResult<string>> UpdateStore([FromBody] Store store)
        {
            if (store == null)
            {
                return BadRequest("Store cannot be null.");
            }

            var updated = await _storeService.UpdateStoreAsync(store);
            if (!updated)
            {
                return NotFound("Store not found.");
            }
            return Ok("Store updated successfully.");
        }

        // DELETE: api/store/{storeId}
        [HttpDelete("{storeId:int}")]
        public async Task<ActionResult<string>> RemoveStore(int storeId)
        {
            var removed = await _storeService.RemoveStoreAsync(storeId);
            if (!removed)
            {
                return NotFound("Store not found.");
            }
            return Ok("Store deleted successfully.");
        }
    }
}