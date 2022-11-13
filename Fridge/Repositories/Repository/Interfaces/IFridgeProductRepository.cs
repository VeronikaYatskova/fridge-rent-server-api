using Fridge.Models;
using Fridge.Models.RoleBasedAuthorization;

namespace Fridge.Repository.Interfaces
{
    public interface IFridgeProductRepository
    {
        Task<FridgeProduct> GetProductByIdAsync(Guid fridgeId, Guid productId, bool trackChanges);
        Task<FridgeProduct> AddProductAsync(Guid fridgeId, Guid productId, int count);
        Task<IEnumerable<FridgeProduct>> GetAllProductsInTheFridgeAsync(Guid fridgeId, bool trackChanges);
        FridgeProduct GetProductById(Guid fridgeId, Guid productId, bool trackChanges);
        void UpdateProduct(FridgeProduct product);
        void DeleteProduct(FridgeProduct product);
        void FillTheFridgeWithProduct(Guid productId, Guid userId);
    }
}
