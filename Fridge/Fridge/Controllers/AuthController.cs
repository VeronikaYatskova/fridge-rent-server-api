using Fridge.Models.DTOs;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Models.DTOs.RenterDtos;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/authorization")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public AuthController(IAuthorizationService service)
        {
            this.authorizationService = service;
        }

        /// <summary>
        /// Add new renter to the system.
        /// </summary>
        /// <param name="renterDto">Email and Password for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("renter/registration")]
        public async Task<IActionResult> RegisterRenter([FromBody] RenterDto renterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = await authorizationService.RegisterRenter(renterDto);

                return Created("api/authorization/renter/register", token);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add new owner to the system.
        /// </summary>
        /// <param name="ownerDto">Name, Email, Password and Telephone for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/registration")]
        public async Task<IActionResult> RegisterOwner([FromBody] OwnerDto ownerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = await authorizationService.RegisterOwner(ownerDto);

                return Created("api/authorization/owner/register", token);
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
        /// Login to the system as a renter.
        /// </summary>
        /// <param name="loginDto">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("renter/login")]
        public IActionResult LoginRenter([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = authorizationService.LoginRenter(loginDto);

                return Created("api/authorization/renter/login", token);
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
        /// Login to the system as an owner.
        /// </summary>
        /// <param name="loginDto">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/login")]
        public IActionResult LoginOwner([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = authorizationService.LoginOwner(loginDto);

                return Created("api/authorization/owner/login", token);
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
