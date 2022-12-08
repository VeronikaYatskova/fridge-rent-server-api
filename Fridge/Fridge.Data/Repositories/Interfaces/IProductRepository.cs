using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(); 
        Task<Product> GetProductByIdAsync(Guid id);
    }
}
