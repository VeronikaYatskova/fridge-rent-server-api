using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges); 
        Task<Product> GetProductByIdAsync(Guid id, bool trackChanges);
        void UpdateProduct(Product product);
    }
}
