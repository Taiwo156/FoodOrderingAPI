using APItask.Service;
using APItask.Core;
using Microsoft.AspNetCore.Mvc;
using APItask.Core.Models;
using APItask.Core.DTOs.Requests;
using APItask.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// // GET: api/orders 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders); 
        }

        /// <summary>
        /// // GET: api/orders/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        /// <summary>
        /// // POST: api/orders
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request,
        [FromServices] IProductRepository productRepository)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(request);
                return Ok(createdOrder);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        /// <summary>
        /// // PUT: api/orders/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedOrder"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order updatedOrder)
        {
            if (id != updatedOrder.OrderId)
            {
                return BadRequest();
            }

            await _orderService.UpdateOrderAsync(updatedOrder);
            return Ok(new { message = "Order updated successfully." });
        }

        /// <summary>
        /// // DELETE: api/orders/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok(new { message = "Order deleted successfully." });
        }
    }

}
