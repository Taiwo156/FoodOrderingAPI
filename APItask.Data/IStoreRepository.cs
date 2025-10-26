using APItask.Core.Models;


namespace APItask.Data
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store?> GetStoreByIdAsync(int storeId);
        Task<Store> AddStoreAsync(Store store);
        Task<bool> UpdateStoreAsync(Store store);
        Task<bool> RemoveStoreAsync(int storeId);
    }
}