using APItask.Core.DTOs.Requests;
using APItask.Core.DTOs.Responses;
using APItask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse?> GetOrderByIdAsync(int orderId);
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
        Task UpdateOrderAsync(Order updatedOrder); 
        Task DeleteOrderAsync(int orderId);
    }
}
