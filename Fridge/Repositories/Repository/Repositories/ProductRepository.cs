using Fridge.Models;
using Fridge.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Repository.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(p => p.Name)
            .ToListAsync();

        public async Task<Product> GetProductByIdAsync(Guid id, bool trackChanges) =>
            await FindByCondition(p => p.Id == id, trackChanges)
            .FirstOrDefaultAsync();

        public void UpdateProduct(Product product) =>
            Update(product);
    }
}
