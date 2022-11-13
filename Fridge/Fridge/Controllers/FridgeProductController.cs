using AutoMapper;
using Fridge.Models.DTOs;
using Fridge.Models.RoleBasedAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models.DTOs;
using Models.Models.RoleBasedAuthorization;
using Repositories.Repository.Interfaces;

namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/products-in-fridge")]
    [ApiController]
    [Authorize(Roles = UserRoles.Renter)]
    public class FridgeProductController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FridgeProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User? _user;
        
        public FridgeProductController(IRepositoryManager repository, ILogger<FridgeProductController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

            var guid = TokenInfo.GetInfo(httpContextAccessor);
            _user = _repository.User.FindUserByCondition(u => u.Id == Guid.Parse(guid), trackChanges: false);
        }

        /// <summary>
        /// Returns a list of products in the fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge to get a products from.</param>
        /// <returns>A list of products in the fridge</returns>
        [HttpGet("fridge/{fridgeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetProductsByFridgeId(Guid fridgeId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductPicture object");
                return UnprocessableEntity(ModelState);
            }

            if (!IsUsersFridge(fridgeId))
            {
                _logger.LogInformation($"You don't have a fridge with id {fridgeId} in your rented.");
                return NotFound();
            }

            var productsId = await _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId, trackChanges: false);
            
            if (productsId is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeId} doesn't exist in the database.");
                return NotFound();
            }

            var productsArray = productsId.ToArray();
            var products = new ProductCountDto[productsArray.Length];
            int i = 0;

            var iterator = productsId.GetEnumerator();
            while (iterator.MoveNext())
            {
                var currentProduct = productsArray[i];
                var productData = await _repository.Product.GetProductByIdAsync(currentProduct.ProductId, trackChanges: false);
                
                products[i++] = new ProductCountDto
                {
                    Id = currentProduct.ProductId,
                    Name = productData.Name,
                    Count = currentProduct.Count,
                };
            }

            return Ok(products);
        }

        /// <summary>
        /// Fill fridges that does not have a specified product with that product.
        /// </summary>
        /// <param name="productId">Specified product.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("product/{productId}/put-in-all-fridges")]
        public async Task<IActionResult> FillTheFridgeWithProduct(Guid productId)
        {
            var product = await _repository.Product.GetProductByIdAsync(productId, trackChanges:false);
            if (product is null)
            {
                _logger.LogInformation($"Product with id {productId} doesn't exist in the database.");
                return NotFound();
            }

            _repository.FridgeProduct.FillTheFridgeWithProduct(productId, _user!.Id);
            await _repository.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Add new product to fridge.
        /// </summary>
        /// <param name="data">Fridge and Product identifiers,count of a product.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("product/new")]
        public async Task<IActionResult> AddProduct([FromBody] FridgeProductDto data)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid model state for the ProductPicture object");
                return UnprocessableEntity(ModelState);
            }

            if (!IsUsersFridge(data.FridgeId))
            {
                _logger.LogInformation($"You don't have a fridge with id {data.FridgeId} in your rented.");
                return NotFound($"You don't have a fridge with id {data.FridgeId} in your rented.");
            }

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(data.FridgeId, trackChanges: false);
            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {data.FridgeId} doesn't exist in the database.");
                return NotFound();
            }

            var product = await _repository.Product.GetProductByIdAsync(data.ProductId, trackChanges: false);
            if (product is null)
            {
                _logger.LogInformation($"Product with id {data.ProductId} doesn't exist in the database.");
                return NotFound();
            }

            if (await GetCountOfProducts(data.FridgeId) + data.Count >= fridge.Capacity)
            {
                _logger.LogError($"The fridge {data.FridgeId} is full.");
                return StatusCode(500, "The fridge is full");
            }

            var newProduct = await _repository.FridgeProduct.AddProductAsync(data.FridgeId, data.ProductId, data.Count == 0 ? product.DefaultQuantity : data.Count);
            var products = await _repository.Product.GetAllProductsAsync(trackChanges:false);
            var productFullName = products.Where(p => p.Id == newProduct.ProductId).FirstOrDefault();

            var pr = new ProductAddDto
            {
                Id = productFullName.Id,
                Name = productFullName.Name,
                Count = newProduct.Count,
            };
            
            await _repository.SaveAsync();

            return Created("api/products-in-fridge/product/new", pr);
        }

        /// <summary>
        /// Method to update a count of product in the fridge.
        /// </summary>
        /// <param name="updateDto">Fridge and Product identifiers,count of a product.</param>
        [HttpPut("product/update")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Update))]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductPicture object");
                return UnprocessableEntity(ModelState);
            }

            var productInFridge = _repository.FridgeProduct.GetProductById(updateDto.FridgeId, updateDto.ProductId, trackChanges: false);
            productInFridge.Count = updateDto.Count;

            if (productInFridge is null)
            {
                _logger.LogInformation($"Product with id: {productInFridge} doesn't exist in the database.");
                return NotFound();
            }

            _repository.FridgeProduct.UpdateProduct(productInFridge);

            await _repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove a specified product from fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge identifier.</param>
        /// <param name="productId">Product identifier.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        [HttpDelete("{fridgeId}/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProductFromFridge(string fridgeId, string productId)
        {
            Console.WriteLine($"{fridgeId} {productId}");
            Guid productGuid = Guid.Parse(productId);
            Guid fridgeGuid = Guid.Parse(fridgeId);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductPicture object");
                return UnprocessableEntity(ModelState);
            }

            if (!IsUsersFridge(fridgeGuid))
            {
                _logger.LogError($"You don't have a fridge with id {fridgeId} in your rented.");
                return NotFound();
            }

            var fridgeProduct = await _repository.FridgeProduct.GetProductByIdAsync(fridgeGuid, productGuid, trackChanges: false);
            if (fridgeProduct is null)
            {
                _logger.LogError($"Fridge with id {fridgeGuid} doesn't contain product with id {productGuid} in the database.");
                return NotFound();
            }

            _repository.FridgeProduct.DeleteProduct(fridgeProduct);

            await _repository.SaveAsync();
            return Ok();
        }

        private async Task<int> GetCountOfProducts(Guid fridgeId)
        {
            var products = await _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId, trackChanges:false);
            int sum = 0;

            foreach (var product in products)
            {
                sum += product.Count;
            }

            return sum;
        }

        private bool IsUsersFridge(Guid fridgeId)
        {
            var usersFridges = _repository.UserFridge.GetUserFridgeRow(_user.Id, fridgeId, trackChanges: false);
            return usersFridges is not null;
        }
    }
}
