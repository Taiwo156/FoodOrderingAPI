using ASPtask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IDeliveryService
    {
        Task<Delivery> GetDeliveryByIdAsync(int deliveryID);
        Task<Delivery> GetDeliveryByOrderIdAsync(int orderID);
        Task<Delivery> CreateDeliveryAsync(Delivery delivery);
        Task<Delivery> UpdateDeliveryStatusAsync(int deliveryID, string status);
        Task<bool> DeleteDeliveryAsync(int deliveryID);
    }
}
