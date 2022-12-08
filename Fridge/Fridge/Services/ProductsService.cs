using Fridge.Controllers;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs.ProductDtos;
using Fridge.Services.Abstracts;

namespace Fridge.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ILogger<ProductsService> _logger;
        private readonly IRepositoryManager _repository;
        private IWebHostEnvironment Environment;

        private readonly Renter user;

        public ProductsService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<ProductsService> logger, IWebHostEnvironment environment)
        {
            _repository = repository;
            _logger = logger;
            Environment = environment;

            if (user is null)
            {
                var tokenInfo = new TokenInfo(repository, httpContextAccessor);
                user = tokenInfo.GetUser().Result;
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _repository.Product.GetAllProductsAsync();

            if (products is null || !products.Any())
            {
                _logger.LogInformation("No products.");
                throw new ArgumentException("No products.");
            }

            return products;
        }

        public async Task<ProductPicture> AddPicture(ProductPictureDto productPictureDto)
        {
            string root = Environment.WebRootPath + "\\Upload\\";
            string extension = Path.GetExtension(productPictureDto.File.FileName);
            string newName = Guid.NewGuid().ToString();
            string fullPath = root + newName + extension;

            if (productPictureDto.File.Length > 0)
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                using FileStream fileStream = System.IO.File.Create(fullPath);

                await productPictureDto.File.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            var newPicture = new ProductPicture
            {
                Id = Guid.NewGuid(),
                ProductId = productPictureDto.ProductId,
                RenterId = user.Id,
                ImageName = productPictureDto.ImageName,
                ImagePath = fullPath,
            };

            _repository.Picture.AddPicture(newPicture);
            await _repository.SaveAsync();

            return newPicture;
        }
    }
}
