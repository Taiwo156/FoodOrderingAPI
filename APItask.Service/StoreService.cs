using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using APItask.Core.Models;
using APItask.Data;
using APItask.Service;
using Microsoft.Extensions.Logging;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;
    private readonly ILogger<StoreService> _logger;

    public StoreService(IStoreRepository storeRepository, ILogger<StoreService> logger)
    {
        _storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IReadOnlyCollection<Store>> GetAllStoresAsync()
    {
        try
        {
            var stores = await _storeRepository.GetAllStoresAsync();
            return stores?.ToList() ?? new List<Store>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving stores");
            throw new StoreServiceException("Failed to retrieve stores", ex);
        }
    }

    public async Task<Store?> GetStoreByIdAsync(int storeId)
    {
        if (storeId <= 0)
        {
            throw new ArgumentException("Invalid store ID", nameof(storeId));
        }

        try
        {
            return await _storeRepository.GetStoreByIdAsync(storeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving store with ID {StoreId}", storeId);
            throw new StoreServiceException($"Failed to retrieve store with ID {storeId}", ex);
        }
    }

    public async Task<Store> AddStoreAsync(Store store)
    {
        if (store == null)
        {
            throw new ArgumentNullException(nameof(store));
        }

        ValidateStore(store);

        try
        {
            return await _storeRepository.AddStoreAsync(store);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding store");
            throw new StoreServiceException("Failed to add store", ex);
        }
    }

    public async Task<bool> UpdateStoreAsync(Store store)
    {
        if (store == null)
        {
            throw new ArgumentNullException(nameof(store));
        }

        // Changed from store.Id to store.StoreId
        if (store.StoreId <= 0)
        {
            throw new ArgumentException("Invalid store ID", nameof(store));
        }

        ValidateStore(store);

        try
        {
            return await _storeRepository.UpdateStoreAsync(store);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating store with ID {StoreId}", store.StoreId);
            throw new StoreServiceException($"Failed to update store with ID {store.StoreId}", ex);
        }
    }

    public async Task<bool> RemoveStoreAsync(int storeId)
    {
        if (storeId <= 0)
        {
            throw new ArgumentException("Invalid store ID", nameof(storeId));
        }

        try
        {
            return await _storeRepository.RemoveStoreAsync(storeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing store with ID {StoreId}", storeId);
            throw new StoreServiceException($"Failed to remove store with ID {storeId}", ex);
        }
    }

    private void ValidateStore(Store store)
    {
        if (string.IsNullOrWhiteSpace(store.StoreName))
        {
            throw new ArgumentException("Store name is required", nameof(store));
        }

        if (string.IsNullOrWhiteSpace(store.StoreAddress))
        {
            throw new ArgumentException("Store address is required", nameof(store));
        }

        // Add validation for other required fields as needed
        if (string.IsNullOrWhiteSpace(store.StoreEmail) || !new EmailAddressAttribute().IsValid(store.StoreEmail))
        {
            throw new ArgumentException("Valid store email is required", nameof(store));
        }
    }
}

public class StoreServiceException : Exception
{
    public StoreServiceException(string message) : base(message) { }
    public StoreServiceException(string message, Exception innerException) : base(message, innerException) { }
}