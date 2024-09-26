using APItask.Service;
using ASPtask.Core;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItask.Properties
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

        // GET: api/delivery/{id}
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

        // GET: api/delivery/order/{orderId}
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

        // POST: api/delivery
        [HttpPost]
        public async Task<IActionResult> CreateDelivery([FromBody] Delivery delivery)
        {
            var newDelivery = await _deliveryService.CreateDeliveryAsync(delivery);
            return Ok(new { message = "Delivery registered successfully." });
        }

        // PUT: api/delivery/{id}
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

        // DELETE: api/delivery/{id}
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
