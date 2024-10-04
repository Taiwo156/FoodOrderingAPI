using APItask.Data;
using APItask.Service;
using ASPtask.Core;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItask.Controllers
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

        /// <summary>
        /// // GET: api/store
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetAllStores()
        {
            var stores = await _storeService.GetAllStoresAsync();
            return Ok(stores);
        }

        /// <summary>
        /// // GET: api/store/{storeId}
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// // POST: api/store
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
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

        /// <summary>
        /// // PUT: api/store
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
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

        /// <summary>
        /// // DELETE: api/store/{storeId}
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
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