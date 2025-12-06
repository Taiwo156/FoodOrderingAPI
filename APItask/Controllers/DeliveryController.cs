using APItask.Service;
using Microsoft.AspNetCore.Mvc;
using APItask.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        /// <summary>
        /// // GET: api/delivery/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryById(int id)
        {
            var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }
            return Ok(delivery);
        }

        /// <summary>
        /// // GET: api/delivery/order/{orderId}
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetDeliveryByOrderId(int orderId)
        {
            var delivery = await _deliveryService.GetDeliveryByOrderIdAsync(orderId);
            if (delivery == null)
            {
                return NotFound();
            }
            return Ok(delivery);
        }

        /// <summary>
        /// // POST: api/delivery
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDelivery([FromBody] Delivery delivery)
        {
            var newDelivery = await _deliveryService.CreateDeliveryAsync(delivery);
            return Ok(new { message = "Delivery registered successfully." });
        }

        /// <summary>
        /// // PUT: api/delivery/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromBody] string status)
        {
            var updatedDelivery = await _deliveryService.UpdateDeliveryStatusAsync(id, status);
            if (updatedDelivery == null)
            {
                return NotFound();
            }
            return Ok(new { message = "Delivery status updated successfully." });
        }

        /// <summary>
        /// // DELETE: api/delivery/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            var result = await _deliveryService.DeleteDeliveryAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { message = "Delivery status deleted successfully." });
        }
    }

}
