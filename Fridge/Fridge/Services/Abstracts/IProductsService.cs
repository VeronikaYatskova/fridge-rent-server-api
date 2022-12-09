using Fridge.Data.Models;
using Fridge.Models.Requests;

namespace Fridge.Services.Abstracts
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<ProductPicture> AddPicture(AddProductPictureModel productPictureDto);
    }
}
