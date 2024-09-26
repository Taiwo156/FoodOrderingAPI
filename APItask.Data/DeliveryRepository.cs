using ASPtask.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Data
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly EssentialProductsDbContext _context;

        public DeliveryRepository(EssentialProductsDbContext context)
        {
            _context = context;
        }

        public async Task<Delivery> GetDeliveryByIdAsync(int deliveryID)
        {
            return await _context.Deliveries.FindAsync(deliveryID);
        }

        public async Task<Delivery> GetDeliveryByOrderIdAsync(int orderID)
        {
            return await _context.Deliveries.FirstOrDefaultAsync(d => d.OrderID == orderID);
        }

        public async Task<Delivery> CreateDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<Delivery> UpdateDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<bool> DeleteDeliveryAsync(int deliveryID)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryID);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }

}
