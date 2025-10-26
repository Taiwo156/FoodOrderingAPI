using APItask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Data
{
    public interface IDeliveryRepository
    {
        Task<Delivery?> GetDeliveryByIdAsync(int deliveryID);
        Task<Delivery?> GetDeliveryByOrderIdAsync(int orderID);
        Task<Delivery> CreateDeliveryAsync(Delivery delivery);
        Task<Delivery> UpdateDeliveryAsync(Delivery delivery);
        Task<bool> DeleteDeliveryAsync(int deliveryID);
    }

}
