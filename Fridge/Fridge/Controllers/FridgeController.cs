using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Services.Abstracts;
using Fridge.Utils.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/fridges")]
    [ValidationFilter]
    [CustomExceptionFilter]
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
            var fridges = await fridgeService.GetFridges();

            return Ok(fridges);
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
            var models = await fridgeService.GetModels();
         
            return Ok(models);
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
            var producers = await fridgeService.GetProducers();
        
            return Ok(producers);
        }

        /// <summary>
        /// Returns a list of fridges that the user has.
        /// </summary>
        /// <returns>A list of fridges that the owner has.</returns>
        /// <response code="200">Returns a list of fridges that the owner has.</response>
        [HttpGet()]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        [Authorize(Roles = $"{UserRoles.Owner}, {UserRoles.Renter}")]
        public async Task<IActionResult> GetUserFridges()
        {
            var fridgesDto = await fridgeService.GetUserFridges();

            return Ok(fridgesDto);
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
            await fridgeService.AddFridge(addFridgeOwnerModel);

            return Created("", "Fridge is added.");
        }

        /// <summary>
        /// Method to rent a fridge.
        /// </summary>
        /// <returns>Rented fridge</returns>
        [HttpPut("{fridgeId}/rent")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Put))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> RentFridge(Guid fridgeId)
        {
            await fridgeService.RentFridge(fridgeId);

            return Ok();
        }

        /// <summary>
        /// Method to return a fridge to its owner.
        /// </summary>
        /// <param name="fridgeId">Guid of a fridge to delete.</param>
        /// <returns>Status Code</returns>
        [HttpPut("{fridgeId}/free")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Put))]
        [Authorize(Roles = UserRoles.Renter)]
        public async Task<IActionResult> Return(Guid fridgeId)
        {
            await fridgeService.ReturnFridge(fridgeId);

            return Ok();
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
            await fridgeService.DeleteFridge(fridgeId);

            return Ok();
        }
    }
}
