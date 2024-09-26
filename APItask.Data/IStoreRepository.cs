using ASPtask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Data
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> GetStoreByIdAsync(int storeId);
        Task<Store> AddStoreAsync(Store store);
        Task<bool> UpdateStoreAsync(Store store);
        Task<bool> RemoveStoreAsync(int storeId);
    }
}