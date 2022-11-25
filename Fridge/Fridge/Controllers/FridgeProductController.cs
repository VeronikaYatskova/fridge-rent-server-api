using Fridge.Data.Models;
using Fridge.Models.DTOs.FridgeProductDto;
using Fridge.Models.DTOs.FridgeProductDtos;
using Fridge.Models.DTOs.ProductDtos;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/products-in-fridge")]
    [ApiController]
    [Authorize(Roles = UserRoles.Renter)]
    public class FridgeProductController : ControllerBase
    {     
        private readonly IFridgeProductService fridgeProductService;

        public FridgeProductController(IFridgeProductService service)
        {
            fridgeProductService = service;
        }

        /// <summary>
        /// Returns a list of products in the fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge to get a products from.</param>
        /// <returns>A list of products in the fridge</returns>
        [HttpGet("fridge/{fridgeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetProductsInFridgeByFridgeId(Guid fridgeId)
        {
            var products = await fridgeProductService.GetProductsByFridgeIdAsync(fridgeId);

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
            await fridgeProductService.FillTheFridgeWithProductAsync(productId);

            return Ok();
        }

        /// <summary>
        /// Add new product to fridge.
        /// </summary>
        /// <param name="fridgeProductDto">Fridge and Product identifiers,count of a product.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("product/new")]
        public async Task<IActionResult> AddProduct([FromBody] FridgeProductDto fridgeProductDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var newProduct = await fridgeProductService.AddProductAsync(fridgeProductDto);

            return Created("api/products-in-fridge/product/new", newProduct);
        }

        /// <summary>
        /// Method to update a count of product in the fridge.
        /// </summary>
        /// <param name="productUpdateDto">Fridge and Product identifiers,count of a product.</param>
        [HttpPut("product/update")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Update))]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductUpdateDto productUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await fridgeProductService.UpdateProductAsync(productUpdateDto);

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
            await fridgeProductService.DeleteProductFromFridgeAsync(fridgeId, productId);

            return Ok();
        }
    }
}
