using APItask.Core.Models;
using APItask.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ILogger<DeliveryService> _logger;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            ILogger<DeliveryService> logger)
        {
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
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
                return await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving delivery with ID {DeliveryId}", deliveryId);
                throw new DeliveryServiceException("Failed to retrieve delivery", ex);
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
                return await _deliveryRepository.GetDeliveryByOrderIdAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving delivery for order ID {OrderId}", orderId);
                throw new DeliveryServiceException("Failed to retrieve delivery by order", ex);
            }
        }

        public async Task<Delivery> CreateDeliveryAsync(Delivery delivery)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery));
            }

            if (delivery.OrderID <= 0)
            {
                throw new ArgumentException("Invalid Order ID", nameof(delivery));
            }

            try
            {
                delivery.CreatedAt = DateTime.UtcNow;
                delivery.UpdatedAt = DateTime.UtcNow;
                delivery.Status = Delivery.DeliveryStatus.Pending; // Default status

                return await _deliveryRepository.CreateDeliveryAsync(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating delivery");
                throw new DeliveryServiceException("Failed to create delivery", ex);
            }
        }

        public async Task<Delivery?> UpdateDeliveryStatusAsync(int deliveryId, string status)
        {
            if (deliveryId <= 0)
            {
                throw new ArgumentException("Invalid delivery ID", nameof(deliveryId));
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Status cannot be empty", nameof(status));
            }

            if (!status.IsValidStatus())
            {
                throw new ArgumentException($"Invalid status: {status}");
            }

            try
            {
                var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
                if (delivery == null)
                {
                    return null;
                }

                delivery.Status = status;
                delivery.UpdatedAt = DateTime.UtcNow;

                return await _deliveryRepository.UpdateDeliveryAsync(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for delivery ID {DeliveryId}", deliveryId);
                throw new DeliveryServiceException("Failed to update delivery status", ex);
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
                return await _deliveryRepository.DeleteDeliveryAsync(deliveryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting delivery with ID {DeliveryId}", deliveryId);
                throw new DeliveryServiceException("Failed to delete delivery", ex);
            }
        }
    }

    public static class DeliveryStatusExtensions
    {
        public static bool IsValidStatus(this string status)
        {
            return status == Delivery.DeliveryStatus.Pending ||
                   status == Delivery.DeliveryStatus.Shipped ||
                   status == Delivery.DeliveryStatus.Delivered ||
                   status == Delivery.DeliveryStatus.Cancelled;
        }
    }

    public class DeliveryServiceException : Exception
    {
        public DeliveryServiceException(string message) : base(message) { }
        public DeliveryServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}