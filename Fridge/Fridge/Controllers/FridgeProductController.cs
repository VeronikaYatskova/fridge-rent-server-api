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
            try
            {
                var products = await fridgeProductService.GetProductsByFridgeIdAsync(fridgeId);

                return Ok(products);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
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
            try
            {
                await fridgeProductService.FillTheFridgeWithProductAsync(productId);

                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var newProduct = await fridgeProductService.AddProductAsync(fridgeProductDto);

                return Created("api/products-in-fridge/product/new", newProduct);
            }
            catch (ArgumentException)
            {
                return BadRequest();
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
        [HttpPut("product/update")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Update))]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductUpdateDto productUpdateDto)
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
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
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
            try
            {
                await fridgeProductService.DeleteProductFromFridgeAsync(fridgeId, productId);

                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
