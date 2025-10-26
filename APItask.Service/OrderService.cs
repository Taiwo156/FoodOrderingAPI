using APItask.Core.DTOs.Requests;
using APItask.Core.DTOs.Responses;
using APItask.Core.Models;
using APItask.Data;

namespace APItask.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Select(MapToOrderResponse).ToList();
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return null;
            return MapToOrderResponse(order);
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            Console.WriteLine($"🔄 Creating order for UserId: {request.UserId}");
            Console.WriteLine($"📦 OrderItems count: {request.OrderItems?.Count ?? 0}");

            var newOrder = new Order
            {
                UserId = request.UserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            foreach (var itemRequest in request.OrderItems)
            {
                Console.WriteLine($"🔍 Processing ProductId: {itemRequest.ProductId}, Quantity: {itemRequest.Quantity}");

                var product = await _productRepository.GetProductAsync(itemRequest.ProductId);
                if (product == null)
                {
                    Console.WriteLine($"❌ Product not found: {itemRequest.ProductId}");
                    throw new ArgumentException($"Product with ID {itemRequest.ProductId} not found");
                }

                Console.WriteLine($"💰 Product Price: {product.Price}");

                var orderItem = new OrderItem
                {
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = product.Price
                };

                newOrder.OrderItems.Add(orderItem);
            }

            Console.WriteLine($"✅ Final OrderItems count: {newOrder.OrderItems.Count}");

            newOrder.CalculateTotals();
            newOrder.TaxAmount = 0;

            Console.WriteLine($"💰 Calculated Total: {newOrder.Total}");

            var createdOrder = await _orderRepository.CreateOrderAsync(newOrder);
            Console.WriteLine($"🎉 Order created with ID: {createdOrder.OrderId}");

            // Map to Response DTO to avoid circular references
            return MapToOrderResponse(createdOrder);
        }

        private OrderResponse MapToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TaxAmount = order.TaxAmount,
                SubTotal = order.SubTotal,
                Total = order.Total,
                Status = order.Status,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemResponse
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product?.Name ?? string.Empty
                }).ToList() ?? new List<OrderItemResponse>(),
                User = order.User != null ? new UserResponseDto
                {
                    Id = order.User.Id,
                    Username = order.User.Username,
                    Email = order.User.Email
                } : null
            };
        }

        public async Task UpdateOrderAsync(Order updatedOrder)
        {
            await _orderRepository.UpdateOrderAsync(updatedOrder);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}