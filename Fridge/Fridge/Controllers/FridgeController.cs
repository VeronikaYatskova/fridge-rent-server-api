using Fridge.Data.Models;
using Fridge.Models.DTOs.FridgeProductDtos;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class FridgeController : ControllerBase
    {
        private readonly IFridgeService fridgeService;
        private readonly IOwnerService ownerService;
        private readonly IRentService rentService;
        private readonly IFridgeProductService fridgeProductService;

        public FridgeController(IFridgeService service, IOwnerService ownerService, IRentService rentService, IFridgeProductService fridgeProductService)
        {
            fridgeService = service;
            this.rentService = rentService;
            this.ownerService = ownerService;
            this.fridgeProductService = fridgeProductService;
        }

        /// <summary>
        /// Returns a list of available fridges.
        /// </summary>
        /// <returns>A list of available fridges.</returns>
        /// <response code="200">Returns a list of available fridges.</response>
        [HttpGet("fridges")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetFridges()
        {
            try
            {
                var fridges = await fridgeService.GetFridges();
                return Ok(fridges);
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
        /// Returns a list of available models.
        /// </summary>
        /// <returns>A list of available models.</returns>
        /// <response code="200">Returns a list of available models.</response>
        [HttpGet("models")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetModels()
        {
            try
            {
                var models = await fridgeService.GetModels();
                return Ok(models);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Returns a list of available producers.
        /// </summary>
        /// <returns>A list of available producers.</returns>
        /// <response code="200">Returns a list of available producers.</response>
        [HttpGet("producers")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetProducers()
        {
            try
            {
                var producers = await fridgeService.GetProducers();
                return Ok(producers);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Returns a list of fridges that the owner has.
        /// </summary>
        /// <returns>A list of fridges that the owner has.</returns>
        /// <response code="200">Returns a list of fridges that the owner has.</response>
        [HttpGet("owner/fridges")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> GetOwnersFridges()
        {
            try
            {
                var fridgesDto = await ownerService.GetOwnersFridges();

                return Ok(fridgesDto);
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
        /// Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.
        /// </summary>
        /// <param name="fridgeId">Fridge to see the info about.</param>
        /// <returns>Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.</returns>
        /// <respose code ="200">Returns an info about the user who rented a fridge.</respose>
        [HttpGet("owner/fridge/{fridgeId}/rent-info")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> GetRentedFridgeInfo(Guid fridgeId)
        {
            try
            {
                var rentDocumentDto = await ownerService.GetRentedFridgeInfo(fridgeId);

                return Ok(rentDocumentDto);
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
        /// Returns a list of fridges that user rented.
        /// </summary>
        /// <returns>A list of fridges</returns>
        [HttpGet("renter/fridges")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> GetRentersFridges()
        {
            try
            {
                var fridges = await rentService.GetRentersFridges();

                return Ok(fridges);
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
        /// Returns a list of products in the fridge.
        /// </summary>
        /// <param name="fridgeId">Fridge to get a products from.</param>
        /// <returns>A list of products in the fridge</returns>
        [HttpGet("renter/fridge/{fridgeId}/products")]
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
        /// Allows to add one more fridge for renting.
        /// </summary>
        /// <param name="ownerAddFridgeDto">Parameters for a new fridge.</param>
        [HttpPost("renter/fridge")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> AddFridge([FromBody] OwnerAddFridgeDto ownerAddFridgeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var fridge = await ownerService.AddFridge(ownerAddFridgeDto);

                return Created("api/owner/fridge/add", fridge);
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
        /// Fill fridges that does not have a specified product with that product.
        /// </summary>
        /// <param name="productId">Specified product.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("renter/fridges/product/{productId}")]
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
        /// Add new product to fridge.
        /// </summary>
        /// <param name="fridgeProductDto">Fridge and Product identifiers,count of a product.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("renter/fridge/product")]
        [Authorize(Roles = UserRoles.Renter)]
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
        /// Method to rent a fridge.
        /// </summary>
        /// <returns>Rented fridge</returns>
        [HttpPost("renter/fridge/{fridgeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> RentFridge(Guid fridgeId)
        {
            try
            {
                await rentService.RentFridge(fridgeId);

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
        [HttpDelete("renter/fridge/{fridgeId}/product/{productId}")]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> DeleteProductFromFridge(string fridgeId, string productId)
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
        /// Allows to delete a fridge.
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <returns></returns>
        [HttpDelete("owner/fridge/{fridgeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> DeleteFridge(Guid fridgeId)
        {
            try
            {
                await ownerService.DeleteFridge(fridgeId);

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
        /// Method to return a fridge to its owner.
        /// </summary>
        /// <param name="fridgeId">Guid of a fridge to delete.</param>
        /// <returns>Status Code</returns>
        [HttpDelete("renter/fridge/{fridgeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> Remove(Guid fridgeId)
        {
            try
            {
                await rentService.Remove(fridgeId);

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
    }
}
