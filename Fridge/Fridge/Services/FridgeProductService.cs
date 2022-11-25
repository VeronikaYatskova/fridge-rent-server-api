using Fridge.Controllers;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs;
using Fridge.Services.Abstracts;
using Models.Models.DTOs;


namespace Fridge.Services
{
    public class FridgeProductService : IFridgeProductService
    {
        private readonly ILogger<FridgeProductService> _logger;
        private readonly IRepositoryManager _repository;
        private readonly User user;

        public FridgeProductService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<FridgeProductService> logger)
        {
            _repository = repository;
            _logger = logger;

            var tokenInfo = new TokenInfo(repository, httpContextAccessor);
            user = tokenInfo.GetUser().Result;
        }

        public async Task<IEnumerable<ProductWithCurrentCountAndNameDto>> GetProductsByFridgeIdAsync(Guid fridgeId)
        {
            if (!IsUsersFridge(fridgeId))
            {
                _logger.LogInformation($"You don't have a fridge with id {fridgeId} in your rented.");
                throw new ArgumentException("Fridge is not found in your fridges");
            }

            var products = await _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId, trackChanges: false);

            if (products is null)
            {
                _logger.LogInformation($"No products in the fridge...");
                throw new ArgumentException("Fridge is empty.");
            }

            var productsWithCount = products.Select(p => new ProductWithCurrentCountAndNameDto
            {
                Id = p.Id,
                Name = _repository.Product.GetProductByIdAsync(p.ProductId, trackChanges: false).Result.Name,
                Count = p.Count,
            });

            return productsWithCount;
        }

        public async Task FillTheFridgeWithProductAsync(Guid productId)
        {
            var product = await _repository.Product.GetProductByIdAsync(productId, trackChanges: false);
            if (product is null)
            {
                _logger.LogInformation($"Product with id {productId} doesn't exist in the database.");
                throw new ArgumentException("Product is not found...");
            }

            _repository.FridgeProduct.FillTheFridgeWithProduct(productId, user.Id);
            await _repository.SaveAsync();
        }

        public async Task<ProductAddDto> AddProductAsync(FridgeProductDto fridgeProductDto)
        {
            if (!IsUsersFridge(fridgeProductDto.FridgeId))
            {
                _logger.LogInformation($"You don't have a fridge with id {fridgeProductDto.FridgeId} in your rented.");
                throw new ArgumentException($"You don't have a fridge with id {fridgeProductDto.FridgeId} in your rented.");
            }

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeProductDto.FridgeId, trackChanges: false);

            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeProductDto.FridgeId} doesn't exist in the database.");
                throw new ArgumentException("Fridge is not found...");
            }

            var product = await _repository.Product.GetProductByIdAsync(fridgeProductDto.ProductId, trackChanges: false);

            if (product is null)
            {
                _logger.LogInformation($"Product with id {fridgeProductDto.ProductId} doesn't exist in the database.");
                throw new ArgumentException("Product is not found...");
            }

            if (await GetCountOfProducts(fridgeProductDto.FridgeId) + fridgeProductDto.Count >= fridge.Capacity)
            {
                _logger.LogError($"The fridge {fridgeProductDto.FridgeId} is full.");
                throw new ArgumentException("The fridge is full");
            }

            var newProduct = await _repository.FridgeProduct.AddProductAsync(fridgeProductDto.FridgeId, fridgeProductDto.ProductId, fridgeProductDto.Count == 0 ? product.DefaultQuantity : fridgeProductDto.Count);
            var products = await _repository.Product.GetAllProductsAsync(trackChanges: false);
            var productFullName = products.Where(p => p.Id == newProduct.ProductId).FirstOrDefault();

            var productWithNameAndCount = new ProductAddDto
            {
                Id = productFullName.Id,
                Name = productFullName.Name,
                Count = newProduct.Count,
            };

            await _repository.SaveAsync();

            return productWithNameAndCount;
        }

        public async Task UpdateProductAsync(ProductUpdateDto productUpdateDto)
        {
            var productInFridge = _repository.FridgeProduct.GetProductById(productUpdateDto.FridgeId, productUpdateDto.ProductId, trackChanges: false);
            productInFridge.Count = productUpdateDto.Count;

            if (productInFridge is null)
            {
                _logger.LogInformation($"Product with id: {productInFridge} doesn't exist in the database.");
                throw new ArgumentException("Product is not found...");
            }

            _repository.FridgeProduct.UpdateProduct(productInFridge);

            await _repository.SaveAsync();
        }

        public async Task DeleteProductFromFridgeAsync(string fridgeId, string productId)
        {
            var productGuid = Guid.Parse(productId);
            var fridgeGuid = Guid.Parse(fridgeId);

            if (!IsUsersFridge(fridgeGuid))
            {
                _logger.LogError($"You don't have a fridge with id {fridgeId} in your rented.");
                throw new ArgumentException("Fridge is not found...");
            }

            var fridgeProduct = await _repository.FridgeProduct.GetProductByIdAsync(fridgeGuid, productGuid, trackChanges: false);
            if (fridgeProduct is null)
            {
                _logger.LogError($"Fridge with id {fridgeGuid} doesn't contain product with id {productGuid} in the database.");
                throw new ArgumentException("Product in the fridge is not found...");
            }

            _repository.FridgeProduct.DeleteProduct(fridgeProduct);

            await _repository.SaveAsync();
        }

        private async Task<int> GetCountOfProducts(Guid fridgeId)
        {
            var products = await _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId, trackChanges: false);
            int sum = 0;

            foreach (var product in products)
            {
                sum += product.Count;
            }

            return sum;
        }

        private bool IsUsersFridge(Guid fridgeId)
        {
            var usersFridges = _repository.UserFridge.GetUserFridgeRow(user.Id, fridgeId, trackChanges: false);
            return usersFridges is not null;
        }
    }
}
