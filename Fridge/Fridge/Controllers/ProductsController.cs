using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize(Roles = UserRoles.Renter)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService service)
        {
            productsService = service;
        }

        /// <summary>
        /// Returns a list of available products.
        /// </summary>
        /// <returns>A list of fridges</returns>
        [HttpGet("available")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productsService.GetProducts();

            return Ok(products);
        }

        /// <summary>
        /// Method that adds a picture to product.
        /// </summary>
        /// <returns>An added picture</returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AddPicture([FromForm] AddProductPictureModel productPictureDto)
        {
            await productsService.AddPictureAsync(productPictureDto);

            return Created("api", productPictureDto.ImageName);
        }
    }
}
