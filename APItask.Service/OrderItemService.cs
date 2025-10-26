using System.Collections.Generic;
using APItask.Core.Models;

namespace APItask.Core
{
    public class OrderItemService
    {
        private readonly OrderItemRepository _orderItemRepository;

        public OrderItemService(OrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public IEnumerable<OrderItem> GetAllOrderItems()
        {
            return _orderItemRepository.GetAll();
        }

        public OrderItem GetOrderItemById(int id)
        {
            return _orderItemRepository.GetById(id);
        }

        public void CreateOrderItem(OrderItem orderItem)
        {
            // Add any business logic here
            _orderItemRepository.Add(orderItem);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            // Add any business logic here
            _orderItemRepository.Update(orderItem);
        }

        public void DeleteOrderItem(int id)
        {
            // Add any business logic here
            _orderItemRepository.Delete(id);
        }
    }
}
