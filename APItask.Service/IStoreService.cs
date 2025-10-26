using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IStoreService
    {
        Task<IReadOnlyCollection<Store>> GetAllStoresAsync();

        Task<Store?> GetStoreByIdAsync(int storeId);

        Task<Store> AddStoreAsync(Store store);

        Task<bool> UpdateStoreAsync(Store store);

        Task<bool> RemoveStoreAsync(int storeId);
    }
}