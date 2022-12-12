using AutoMapper;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.Requests;
using Fridge.Models.Responses;
using Fridge.Services.Abstracts;


namespace Fridge.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ILogger<ProductsService> logger;
        private readonly IRepositoryManager repository;
        private IWebHostEnvironment environment;
        private IMapper mapper;

        private TokenInfo tokenInfo;
        private Renter? renter;

        public ProductsService(IRepositoryManager repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<ProductsService> logger, IWebHostEnvironment environment)
        {
            this.repository = repository;
            this.logger = logger;
            this.environment = environment;
            this.mapper = mapper;

            tokenInfo = new TokenInfo(repository, httpContextAccessor);
        }

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            var products = await repository.Product.GetAllProductsAsync();

            if (products is null || !products.Any())
            {
                logger.LogInformation("No products.");
                throw new ArgumentException("No products.");
            }

            return mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task AddPictureAsync(AddProductPictureModel addProductPictureModel)
        {
            renter = await tokenInfo.GetUser();

            string root = environment.WebRootPath + "\\Upload\\";
            string extension = Path.GetExtension(addProductPictureModel.File.FileName);
            string newName = Guid.NewGuid().ToString();
            string fullPath = root + newName + extension;

            if (addProductPictureModel.File.Length > 0)
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                using FileStream fileStream = File.Create(fullPath);

                await addProductPictureModel.File.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            var newPicture = new ProductPicture
            {
                Id = Guid.NewGuid(),
                ProductId = addProductPictureModel.ProductId,
                RenterId = renter.Id,
                ImageName = addProductPictureModel.ImageName,
                ImagePath = fullPath,
            };

            repository.Picture.AddPicture(newPicture);
            await repository.SaveAsync();
        }
    }
}
