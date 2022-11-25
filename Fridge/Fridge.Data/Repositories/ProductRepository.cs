using Fridge.Data.Context;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Data.Repositories
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
