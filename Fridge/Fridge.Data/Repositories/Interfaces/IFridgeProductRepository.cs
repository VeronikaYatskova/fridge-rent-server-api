﻿
using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IFridgeProductRepository
    {
        Task<FridgeProduct> GetProductByIdAsync(Guid fridgeId, Guid productId);
        Task<FridgeProduct> AddProductAsync(Guid fridgeId, Guid productId, int count);
        Task<IEnumerable<FridgeProduct>> GetAllProductsInTheFridgeAsync(Guid fridgeId);
        FridgeProduct GetProductById(Guid fridgeId, Guid productId);
        void UpdateProduct(FridgeProduct product);
        void DeleteProduct(FridgeProduct product);
        void FillTheFridgeWithProduct(Guid productId, Guid userId);
    }
}
