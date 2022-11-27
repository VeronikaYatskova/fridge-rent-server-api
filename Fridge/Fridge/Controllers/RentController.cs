using Fridge.Data.Models;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Controllers
{
    [Route("api/rent")]
    [ApiController]
    [Authorize(Roles = UserRoles.Renter)]
    public class RentController : ControllerBase
    {
        private readonly IRentService rentService;

        public RentController(IRentService service)
        {
            this.rentService = service;
        }

        /// <summary>
        /// Returns a list of fridges that user rented.
        /// </summary>
        /// <returns>A list of fridges</returns>
        [HttpGet("fridges/rented")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetUsersFridges()
        {
            try
            {
                var fridges = await rentService.GetUsersFridges();

                return Ok(fridges);
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
        /// Method to rent a fridge.
        /// </summary>
        /// <returns>Rented fridge</returns>
        [HttpPost("fridge/{fridgeId}/rent")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> RentFridge(Guid fridgeId)
        {
            try
            {
                await rentService.RentFridge(fridgeId);

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
        /// Method to return a fridge to its owner.
        /// </summary>
        /// <param name="fridgeId">Guid of a fridge to delete.</param>
        /// <returns>Status Code</returns>
        [HttpDelete("fridge/{fridgeId}/return")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> Remove(Guid fridgeId)
        {
            try
            {
                await rentService.Remove(fridgeId);

                return Ok();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
