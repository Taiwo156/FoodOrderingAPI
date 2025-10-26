using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EssentialProductsDbContext _context;

        public OrderRepository(EssentialProductsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyCollection<Order>> GetAllOrdersAsync()
        {
            var orders = await _context.Order
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .AsNoTracking()
                .ToListAsync();

            // Debug logging
            foreach (var order in orders)
            {
                Console.WriteLine($"Order {order.OrderId} has {order.OrderItems?.Count ?? 0} items");
                Console.WriteLine($"Order {order.OrderId} user is null: {order.User == null}");
            }

            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Order
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product) 
                .Include(o => o.User) // Include User
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> CreateOrderAsync(Order newOrder)
        {
            if (newOrder == null)
            {
                throw new ArgumentNullException(nameof(newOrder));
            }

            try
            {
                // Validate all ProductIds exist before saving
                foreach (var item in newOrder.OrderItems)
                {
                    var product = await _context.Product
                        .FirstOrDefaultAsync(p => p.ProductId == item.ProductId && p.IsActive);

                    if (product == null)
                    {
                        throw new ArgumentException(
                            $"ProductId {item.ProductId} does not exist or is not active."
                        );
                    }

                    // ✅ Set the UnitPrice from the Product table
                    item.UnitPrice = product.Price;
                }

                // ✅ Calculate totals BEFORE saving
                newOrder.CalculateTotals();

                await _context.Order.AddAsync(newOrder);
                await _context.SaveChangesAsync();

                // ✅ CRITICAL: Reload the order with ALL related data included
                var createdOrder = await _context.Order
                    .Include(o => o.OrderItems) // Include OrderItems
                        .ThenInclude(oi => oi.Product) // Include Product for each OrderItem
                    .Include(o => o.User) // Include User
                    .AsNoTracking() // Optional: prevents tracking issues
                    .FirstOrDefaultAsync(o => o.OrderId == newOrder.OrderId);

                return createdOrder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateOrderAsync(Order updatedOrder)
        {
            if (updatedOrder == null)
            {
                throw new ArgumentNullException(nameof(updatedOrder));
            }

            if (updatedOrder.OrderId <= 0)
            {
                throw new ArgumentException("Invalid Order ID", nameof(updatedOrder));
            }

            _context.Order.Update(updatedOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentException("Invalid Order ID", nameof(orderId));
            }

            var order = await _context.Order.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}