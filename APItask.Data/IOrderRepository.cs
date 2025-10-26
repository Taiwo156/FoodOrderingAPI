using APItask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Data
{
    public interface IOrderRepository
    {
        Task<IReadOnlyCollection<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order> CreateOrderAsync(Order newOrder);
        Task UpdateOrderAsync(Order updatedOrder);
        Task<bool> DeleteOrderAsync(int orderId);
    }

}
