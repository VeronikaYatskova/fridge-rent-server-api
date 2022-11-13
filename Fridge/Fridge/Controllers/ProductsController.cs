using AutoMapper;
using Fridge.Models;
using Fridge.Models.DTOs;
using Fridge.Models.RoleBasedAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models.RoleBasedAuthorization;
using Repositories.Repository.Interfaces;

namespace Fridge.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize(Roles = UserRoles.Renter)]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private IWebHostEnvironment Environment;
        private readonly User user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsController(IRepositoryManager repository, ILogger<ProductsController> logger, IMapper mapper, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            Environment = environment;

            var guid = TokenInfo.GetInfo(httpContextAccessor);
            user = _repository.User.FindUserByCondition(u => u.Id == Guid.Parse(guid), trackChanges: false);
        }

        /// <summary>
        /// Returns a list of available products.
        /// </summary>
        /// <returns>A list of fridges</returns>
        [HttpGet("fridges/rented")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.Product.GetAllProductsAsync(trackChanges: false);

            if (products is null || !products.Any())
            {
                _logger.LogInformation("No products.");
                return NotFound("No products.");
            }

            return Ok(products);
        }

        /// <summary>
        /// Method that adds a picture to product.
        /// </summary>
        /// <returns>An added picture</returns>
        [HttpPost("product/picture")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AddPicture([FromForm]ProductPictureDto productPicture)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductPictureDto object");
                return UnprocessableEntity(ModelState);
            }

            string root = Environment.WebRootPath + "\\Upload\\";
            string extension = Path.GetExtension(productPicture.File.FileName);
            string newName = Guid.NewGuid().ToString();
            string fullPath = root + newName + extension;

            Console.WriteLine(root);
            if (productPicture.File.Length > 0)
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                using FileStream fileStream = System.IO.File.Create(fullPath);
                
                await productPicture.File.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            var newPicture = new ProductPicture
            {
                Id = Guid.NewGuid(),
                ProductId = productPicture.ProductId,
                UserId = user.Id,
                ImageName = productPicture.ImageName,
                ImagePath = fullPath,
            };

            _repository.Picture.AddPicture(newPicture);
            await _repository.SaveAsync();

            return Ok(newPicture);
        }
    }
}
