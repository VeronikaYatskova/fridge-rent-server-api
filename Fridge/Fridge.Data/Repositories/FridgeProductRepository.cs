using Fridge.Data.Context;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Data.Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<FridgeProduct?> GetProductByIdAsync(Guid fridgeId, Guid productId) => 
            await FindByCondition(f => f.FridgeId == fridgeId && f.ProductId == productId)
            .FirstOrDefaultAsync();

        public FridgeProduct? GetProductById(Guid fridgeId, Guid productId) =>
            FindByCondition(f => f.FridgeId == fridgeId && f.ProductId == productId)
            .FirstOrDefault();

        public async Task<IEnumerable<FridgeProduct>> GetAllProductsInTheFridgeAsync(Guid fridgeId) =>
            await FindByCondition(f => f.FridgeId == fridgeId)
            .ToListAsync();

        public async Task<FridgeProduct> AddProductAsync(Guid fridgeId, Guid productId, int count)
        {
            var fridgeProduct = await GetProductByIdAsync(fridgeId, productId);
            
            if (fridgeProduct is not null)
            {
                fridgeProduct.Count += count;
                Update(fridgeProduct);
                return fridgeProduct;
            }
            else
            {
                var fr = new FridgeProduct 
                { 
                    Id = Guid.NewGuid(), 
                    FridgeId = fridgeId, 
                    ProductId = productId, 
                    Count = count 
                };

                Create(fr);
                return fr;
            }
        }

        public void UpdateProduct(FridgeProduct product) => Update(product);

        public void DeleteProduct(FridgeProduct product) => Delete(product);

        public void FillTheFridgeWithProduct(Guid productId, Guid userId) => ExecuteStoredProcedure(productId, userId);
    }
}
