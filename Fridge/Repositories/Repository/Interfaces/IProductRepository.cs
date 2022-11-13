using Fridge.Models;

namespace Fridge.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges); 
        Task<Product> GetProductByIdAsync(Guid id, bool trackChanges);
        void UpdateProduct(Product product);
    }
}
