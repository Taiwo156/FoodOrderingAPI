using System.Collections.Generic;
using System.Linq;
using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace APItask.Core
{
    public class OrderItemRepository
    {
        private readonly DbContext _context;

        public OrderItemRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return _context.Set<OrderItem>().ToList();
        }

        public OrderItem GetById(int id)
        {
            return _context.Set<OrderItem>().Find(id);
        }

        public void Add(OrderItem orderItem)
        {
            _context.Set<OrderItem>().Add(orderItem);
            _context.SaveChanges();
        }

        public void Update(OrderItem orderItem)
        {
            _context.Set<OrderItem>().Update(orderItem);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var orderItem = _context.Set<OrderItem>().Find(id);
            if (orderItem != null)
            {
                _context.Set<OrderItem>().Remove(orderItem);
                _context.SaveChanges();
            }
        }
    }
}
