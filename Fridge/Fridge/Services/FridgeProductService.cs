using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.Requests;
using Fridge.Models.Responses;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Http;


namespace Fridge.Services
{
    public class FridgeProductService : IFridgeProductService
    {
        private readonly ILogger<FridgeProductService> logger;
        private readonly IRepositoryManager repository;

        private readonly TokenInfo tokenInfo;
        private User? renter;

        public FridgeProductService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<FridgeProductService> logger, IConfiguration configuration)
        {
            this.repository = repository;
            this.logger = logger;

            tokenInfo = new TokenInfo(repository, httpContextAccessor, configuration);
        }

        public async Task<IEnumerable<ProductWithCurrentCountAndNameModel>> GetProductsByFridgeIdAsync(Guid fridgeId)
        {
            renter = await tokenInfo.GetUser();

            if (!IsRentersFridge(fridgeId))
            {
                logger.LogInformation($"You don't have a fridge with id {fridgeId} in your rented.");
                throw new ArgumentException("Fridge is not found in your fridges");
            }

            var products = await repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId);

            if (products is null)
            {
                logger.LogInformation($"No products in the fridge...");
                throw new ArgumentException("Fridge is empty.");
            }

            var productsWithCount = products.Select(p => new ProductWithCurrentCountAndNameModel
            {
                Id = p.ProductId,
                Name = p.Product.Name,
                Count = p.Count,
            });

            return productsWithCount;
        }

        public async Task FillTheFridgeWithProductAsync(Guid productId)
        {
            renter = await tokenInfo.GetUser();

            var product = await repository.Product.GetProductByIdAsync(productId);
            if (product is null)
            {
                logger.LogInformation($"Product with id {productId} doesn't exist in the database.");
                throw new ArgumentException("Product is not found...");
            }

            repository.FridgeProduct.FillTheFridgeWithProduct(productId, renter.Id);
            await repository.SaveAsync();
        }

        public async Task AddProductAsync(AddProductModel addProductModel)
        {
            renter = await tokenInfo.GetUser();

            var isRentersFridge = IsRentersFridge(addProductModel.FridgeId);
            if (!isRentersFridge)
            {
                logger.LogInformation($"You don't have a fridge with id {addProductModel.FridgeId} in your rented.");
                throw new ArgumentException($"You don't have a fridge with id {addProductModel.FridgeId} in your rented.");
            }

            var fridge = await repository.Fridge.GetFridgeByIdAsync(addProductModel.FridgeId);

            if (fridge is null)
            {
                logger.LogInformation($"Fridge with id {addProductModel.FridgeId} doesn't exist in the database.");
                throw new ArgumentException("Fridge is not found...");
            }

            var product = await repository.Product.GetProductByIdAsync(addProductModel.ProductId);

            if (product is null)
            {
                logger.LogInformation($"Product with id {addProductModel.ProductId} doesn't exist in the database.");
                throw new ArgumentException("Product is not found...");
            }

            if (await GetCountOfProducts(addProductModel.FridgeId) + addProductModel.Count >= fridge.Capacity)
            {
                logger.LogError($"The fridge {addProductModel.FridgeId} is full.");
                throw new ArgumentException("The fridge is full");
            }

            var newProduct = await repository.FridgeProduct.AddProductAsync(addProductModel.FridgeId, addProductModel.ProductId, addProductModel.Count == 0 ? product.DefaultQuantity : addProductModel.Count);
            var products = await repository.Product.GetAllProductsAsync();
            var productFullName = products.Where(p => p.Id == newProduct.ProductId).FirstOrDefault();

            var productWithNameAndCount = new ProductWithCurrentCountAndNameModel
            {
                Id = productFullName.Id,
                Name = productFullName.Name,
                Count = newProduct.Count,
            };

            await repository.SaveAsync();
        }

        public async Task UpdateProductAsync(UpdateProductModel updateProductModel)
        {
            renter = await tokenInfo.GetUser();

            var productInFridge = repository.FridgeProduct.GetProductById(updateProductModel.FridgeId, updateProductModel.ProductId);
            productInFridge.Count = updateProductModel.Count;

            if (productInFridge is null)
            {
                logger.LogInformation($"Product with id: {productInFridge} doesn't exist in the database.");
                throw new ArgumentException("Product is not found...");
            }

            repository.FridgeProduct.UpdateProduct(productInFridge);

            await repository.SaveAsync();
        }

        public async Task DeleteProductFromFridgeAsync(Guid fridgeId, Guid productId)
        {
            renter = await tokenInfo.GetUser();

            if (!IsRentersFridge(fridgeId))
            {
                logger.LogError($"You don't have a fridge with id {fridgeId} in your rented.");
                throw new ArgumentException("Fridge is not found...");
            }

            var fridgeProduct = await repository.FridgeProduct.GetProductByIdAsync(fridgeId, productId);
            if (fridgeProduct is null)
            {
                logger.LogError($"Fridge with id {fridgeId} doesn't contain product with id {productId} in the database.");
                throw new ArgumentException("Product in the fridge is not found...");
            }

            repository.FridgeProduct.DeleteProduct(fridgeProduct);

            await repository.SaveAsync();
        }

        private async Task<int> GetCountOfProducts(Guid fridgeId)
        {
            var products = await repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId);
            var sum = products.Select(f => f.Count).Sum();
            
            return sum;
        }

        private bool IsRentersFridge(Guid fridgeId) => 
            renter?.RenterFridges.FirstOrDefault(f => f.Id == fridgeId) is not null;
    }
}
