using Fridge.Data.Models;
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
        private readonly IFridgeProductService fridgeProductService;

        public ProductsController(IProductsService service, IFridgeProductService fridgeProductService)
        {
            productsService = service;
            this.fridgeProductService = fridgeProductService;
        }

        /// <summary>
        /// Returns a list of available products.
        /// </summary>
        /// <returns>A list of fridges</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await productsService.GetProducts();

                return Ok(products);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Method that adds a picture to product.
        /// </summary>
        /// <returns>An added picture</returns>
        [HttpPost("product")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AddPicture([FromForm]AddProductPictureModel productPictureDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }

                await productsService.AddPictureAsync(productPictureDto);

                return Created("api/product", productPictureDto.ImageName);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Method to update a count of product in the fridge.
        /// </summary>
        /// <param name="productUpdateDto">Fridge and Product identifiers,count of a product.</param>
        [HttpPut("product")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Update))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> UpdateProductAsync([FromBody] UpdateProductModel productUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                await fridgeProductService.UpdateProductAsync(productUpdateDto);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
