using APItask.Service;
using ASPtask.Core;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<Order>> CreateOrder(Order newOrder)
        {
            var createdOrder = await _orderService.CreateOrderAsync(newOrder);
            return Ok(new { message = "Order created successfully." });
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
