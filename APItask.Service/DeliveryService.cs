using APItask.Data;
using ASPtask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<Delivery> GetDeliveryByIdAsync(int deliveryID)
        {
            return await _deliveryRepository.GetDeliveryByIdAsync(deliveryID);
        }

        public async Task<Delivery> GetDeliveryByOrderIdAsync(int orderID)
        {
            return await _deliveryRepository.GetDeliveryByOrderIdAsync(orderID);
        }

        public async Task<Delivery> CreateDeliveryAsync(Delivery delivery)
        {
            delivery.CreatedAt = DateTime.Now;
            delivery.UpdatedAt = DateTime.Now;
            return await _deliveryRepository.CreateDeliveryAsync(delivery);
        }

        public async Task<Delivery> UpdateDeliveryStatusAsync(int deliveryID, string status)
        {
            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryID);
            if (delivery == null)
            {
                return null;
            }

            delivery.Status = status;
            delivery.UpdatedAt = DateTime.Now;
            return await _deliveryRepository.UpdateDeliveryAsync(delivery);
        }

        public async Task<bool> DeleteDeliveryAsync(int deliveryID)
        {
            return await _deliveryRepository.DeleteDeliveryAsync(deliveryID);
        }
    }
}
