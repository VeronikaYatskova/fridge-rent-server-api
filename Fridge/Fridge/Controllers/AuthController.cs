using Fridge.Models.DTOs;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Models.DTOs.UserDtos;
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
        /// Add new user to the system.
        /// </summary>
        /// <param name="userDto">Email and Password for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("user/register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = await authorizationService.RegisterUser(userDto);

                return Created("api/authorization/user/register", token);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Add new owner to the system.
        /// </summary>
        /// <param name="ownerDto">Name, Email, Password and Telephone for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/register")]
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
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Login to the system as a user.
        /// </summary>
        /// <param name="loginDto">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("user/login")]
        public IActionResult LoginUser([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = authorizationService.LoginUser(loginDto);

                return Created("api/authorization/user/login", token);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return NotFound();
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
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}
