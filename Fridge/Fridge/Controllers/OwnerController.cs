using AutoMapper;
using Fridge.Data.Models;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/owner")]
    [ApiController]
    [Authorize(Roles = UserRoles.Owner)]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService ownerService;

        public OwnerController(IOwnerService service)
        {
            ownerService = service;
        }

        /// <summary>
        /// Returns a list of fridges that the owner has.
        /// </summary>
        /// <returns>A list of fridges that the owner has.</returns>
        /// <response code="200">Returns a list of fridges that the owner has.</response>
        [HttpGet("fridges")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetOwnersFridges()
        {
            try
            {
                var fridgesDto = await ownerService.GetOwnersFridges();

                return Ok(fridgesDto);
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
        /// Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.
        /// </summary>
        /// <param name="fridgeId">Fridge to see the info about.</param>
        /// <returns>Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.</returns>
        /// <respose code ="200">Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.</respose>
        [HttpGet("fridge/{fridgeId}/rent-info")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetRentedFridgeInfo(Guid fridgeId)
        {
            try
            {
                var rentDocumentDto = await ownerService.GetRentedFridgeInfo(fridgeId);

                return Ok(rentDocumentDto);
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
        /// Allows to add one more fridge for renting.
        /// </summary>
        /// <param name="ownerAddFridgeDto">Parameters for a new fridge.</param>
        [HttpPost("fridge/add")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
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
        /// Allows to delete a fridge.
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <returns></returns>
        [HttpDelete("fridge/{fridgeId}/remove")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteFridge(Guid fridgeId)
        {
            try
            {
                await ownerService.DeleteFridge(fridgeId);

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
