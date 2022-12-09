using Fridge.Data.Models;
using Fridge.Models.Requests;
using Fridge.Models.Responses;

namespace Fridge.Services.Abstracts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductModel>> GetProducts();

        Task AddPicture(AddProductPictureModel productPictureDto);
    }
}
