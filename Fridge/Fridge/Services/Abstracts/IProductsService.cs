using Fridge.Data.Models;
using Fridge.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Services.Abstracts
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<ProductPicture> AddPicture(ProductPictureDto productPictureDto);
    }
}
