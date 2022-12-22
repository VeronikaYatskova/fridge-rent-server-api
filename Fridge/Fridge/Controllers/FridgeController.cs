using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/fridges")]
    [ApiController]
    public class FridgeController : ControllerBase
    {
        private readonly IFridgeService fridgeService;

        public FridgeController(IFridgeService service)
        {
            fridgeService = service;
        }

        /// <summary>
        /// Returns a list of available fridges.
        /// </summary>
        /// <returns>A list of available fridges.</returns>
        /// <response code="200">Returns a list of available fridges.</response>
        [HttpGet("available")]
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
        [HttpGet()]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        [Authorize(Roles = $"{UserRoles.Owner}, {UserRoles.Renter}")]
        public async Task<IActionResult> GetUserFridges()
        {
            try
            {
                var fridgesDto = await fridgeService.GetUserFridges();

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
        /// Allows to add one more fridge for renting.
        /// </summary>
        /// <param name="addFridgeOwnerModel">Parameters for a new fridge.</param>
        [HttpPost()]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> AddFridge([FromBody] AddFridgeOwnerModel addFridgeOwnerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }

                await fridgeService.AddFridge(addFridgeOwnerModel);

                return Created("api/owner/fridge", "Fridge is added.");
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
        [HttpPut("{fridgeId}/rent")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> RentFridge(Guid fridgeId)
        {
            try
            {
                await fridgeService.RentFridge(fridgeId);

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
        /// Allows to delete a fridge.
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <returns></returns>
        [HttpDelete("{fridgeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> DeleteFridge(Guid fridgeId)
        {
            try
            {
                await fridgeService.DeleteFridge(fridgeId);

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
        [HttpPut("{fridgeId}/free")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> Remove(Guid fridgeId)
        {
            try
            {
                await fridgeService.ReturnFridge(fridgeId);

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
