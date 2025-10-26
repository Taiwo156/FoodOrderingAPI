using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace APItask.Data
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly EssentialProductsDbContext _context;
        private readonly ILogger<DeliveryRepository> _logger;

        public DeliveryRepository(
            EssentialProductsDbContext context,
            ILogger<DeliveryRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Delivery?> GetDeliveryByIdAsync(int deliveryId)
        {
            if (deliveryId <= 0)
            {
                throw new ArgumentException("Invalid delivery ID", nameof(deliveryId));
            }

            try
            {
                return await _context.Deliveries
                    .Include(d => d.Order)  // Eager loading if needed
                    .FirstOrDefaultAsync(d => d.DeliveryID == deliveryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving delivery with ID {DeliveryId}", deliveryId);
                throw new RepositoryException("Failed to retrieve delivery", ex);
            }
        }

        public async Task<Delivery?> GetDeliveryByOrderIdAsync(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentException("Invalid order ID", nameof(orderId));
            }

            try
            {
                return await _context.Deliveries
                    .Include(d => d.Order)  // Eager loading if needed
                    .FirstOrDefaultAsync(d => d.OrderID == orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving delivery for order ID {OrderId}", orderId);
                throw new RepositoryException("Failed to retrieve delivery by order", ex);
            }
        }

        public async Task<Delivery> CreateDeliveryAsync(Delivery delivery)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery));
            }

            try
            {
                delivery.CreatedAt = DateTime.UtcNow;
                delivery.UpdatedAt = DateTime.UtcNow;

                await _context.Deliveries.AddAsync(delivery);
                await _context.SaveChangesAsync();
                return delivery;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating delivery");
                throw new RepositoryException("Failed to create delivery", ex);
            }
        }

        public async Task<Delivery> UpdateDeliveryAsync(Delivery delivery)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery));
            }

            if (delivery.DeliveryID <= 0)
            {
                throw new ArgumentException("Invalid delivery ID", nameof(delivery));
            }

            try
            {
                delivery.UpdatedAt = DateTime.UtcNow;
                _context.Deliveries.Update(delivery);
                await _context.SaveChangesAsync();
                return delivery;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery with ID {DeliveryId}", delivery.DeliveryID);
                throw new RepositoryException("Failed to update delivery", ex);
            }
        }

        public async Task<bool> DeleteDeliveryAsync(int deliveryId)
        {
            if (deliveryId <= 0)
            {
                throw new ArgumentException("Invalid delivery ID", nameof(deliveryId));
            }

            try
            {
                var delivery = await _context.Deliveries.FindAsync(deliveryId);
                if (delivery == null)
                {
                    return false;
                }

                _context.Deliveries.Remove(delivery);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting delivery with ID {DeliveryId}", deliveryId);
                throw new RepositoryException("Failed to delete delivery", ex);
            }
        }
    }

    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception inner) : base(message, inner) { }
    }
}