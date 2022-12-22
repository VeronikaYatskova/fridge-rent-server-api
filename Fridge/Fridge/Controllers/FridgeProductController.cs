using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Controllers
{
    [ApiController]
    public class FridgeProductController : ControllerBase
    {
        private readonly IFridgeProductService fridgeProductService;
        
        public FridgeProductController(IFridgeProductService fridgeProductService)
        {
            this.fridgeProductService = fridgeProductService;
        }

        /// <summary>
        /// Returns a list of products in the fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge to get a products from.</param>
        /// <returns>A list of products in the fridge</returns>
        [HttpGet("fridges/{fridgeId}/products")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> GetProductsInFridgeByFridgeId(Guid fridgeId)
        {
            try
            {
                var products = await fridgeProductService.GetProductsByFridgeIdAsync(fridgeId);

                return Ok(products);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
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
        [HttpPost("fridges/products/{productId}")]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> FillTheFridgeWithProduct(Guid productId)
        {
            try
            {
                await fridgeProductService.FillTheFridgeWithProductAsync(productId);

                return Ok();
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
        /// Add new product to a fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge to add a product to.</param>
        /// <param name="productId">Product to add to a fridge.</param>
        /// <param name="count">Count of items.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("fridges/{fridgeId}/products/{productId}")]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> AddProduct(Guid fridgeId, Guid productId, [FromBody] int count)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }

                await fridgeProductService.AddProductAsync(new AddProductModel
                {
                    FridgeId = fridgeId,
                    ProductId = productId,
                    Count = count,
                });

                return Ok();
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
        /// Remove a specified product from fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge identifier.</param>
        /// <param name="productId">Product identifier.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        [HttpDelete("fridges/{fridgeId}/products/{productId}")]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> DeleteProductFromFridge(Guid fridgeId, Guid productId)
        {
            try
            {
                await fridgeProductService.DeleteProductFromFridgeAsync(fridgeId, productId);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Update count of product in the fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge to add a product to.</param>
        /// <param name="productId">Product to add to a fridge.</param>
        /// <param name="count">Count of items.</param>
        /// <returns></returns>
        [HttpPut("fridges/{fridgeId}/products/{productId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Update))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> UpdateProductAsync(Guid fridgeId, Guid productId, [FromBody] int count)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                await fridgeProductService.UpdateProductAsync(new UpdateProductModel
                {
                    FridgeId = fridgeId,
                    ProductId = productId,
                    Count = count,
                });

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
