using Fridge.Models.Requests;
using Fridge.Models.Responses;

namespace Fridge.Services.Abstracts
{
    public interface IFridgeProductService
    {
        Task<IEnumerable<ProductWithCurrentCountAndNameModel>> GetProductsByFridgeIdAsync(Guid fridgeId);

        Task FillTheFridgeWithProductAsync(Guid productId);

        Task AddProductAsync(AddProductModel addProductModel);

        Task UpdateProductAsync(UpdateProductModel updateProductModel);

        Task DeleteProductFromFridgeAsync(string fridgeId, string productId);
    }
}
