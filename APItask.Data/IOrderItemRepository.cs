using System.Collections.Generic;
using APItask.Core.Models;

namespace APItask.Core
{
    public interface IOrderItemRepository
    {
        IEnumerable<OrderItem> GetAll();
        OrderItem GetById(int id);
        void Add(OrderItem orderItem);
        void Update(OrderItem orderItem);
        void Delete(int id);
    }
}
