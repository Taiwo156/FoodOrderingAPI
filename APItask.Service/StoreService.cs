using APItask.Data;
using ASPtask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _storeRepository.GetAllStoresAsync();
        }

        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _storeRepository.GetStoreByIdAsync(storeId);
        }

        public async Task<Store> AddStoreAsync(Store store)
        {
            return await _storeRepository.AddStoreAsync(store);
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            return await _storeRepository.UpdateStoreAsync(store);
        }

        public async Task<bool> RemoveStoreAsync(int storeId)
        {
            return await _storeRepository.RemoveStoreAsync(storeId);
        }
    }
}