using AutoMapper;
using Fridge.Data.Models;
using Fridge.Models.DTOs;
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
        [HttpGet("all-products")]
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
        [HttpPost("product/picture")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AddPicture([FromForm]ProductPictureDto productPictureDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var newPicture = productsService.AddPicture(productPictureDto);
            
            return Ok(newPicture);
        }
    }
}
