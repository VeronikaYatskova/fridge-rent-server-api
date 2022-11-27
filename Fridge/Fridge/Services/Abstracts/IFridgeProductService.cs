using Fridge.Models.DTOs.FridgeProductDto;
using Fridge.Models.DTOs.FridgeProductDto.FridgeProductDto;
using Fridge.Models.DTOs.FridgeProductDtos;
using Fridge.Models.DTOs.ProductDtos;

namespace Fridge.Services.Abstracts
{
    public interface IFridgeProductService
    {
        Task<IEnumerable<ProductWithCurrentCountAndNameDto>> GetProductsByFridgeIdAsync(Guid fridgeId);

        Task FillTheFridgeWithProductAsync(Guid productId);

        Task<AddProductDto> AddProductAsync(FridgeProductDto data);

        Task UpdateProductAsync(ProductUpdateDto productUpdateDto);

        Task DeleteProductFromFridgeAsync(string fridgeId, string productId);
    }
}
