using Fridge.Models.DTOs;
using Models.Models.DTOs;

namespace Fridge.Services.Abstracts
{
    public interface IFridgeProductService
    {
        Task<IEnumerable<ProductWithCurrentCountAndNameDto>> GetProductsByFridgeIdAsync(Guid fridgeId);

        Task FillTheFridgeWithProductAsync(Guid productId);

        Task<ProductAddDto> AddProductAsync(FridgeProductDto data);

        Task UpdateProductAsync(ProductUpdateDto productUpdateDto);

        Task DeleteProductFromFridgeAsync(string fridgeId, string productId);
    }
}
