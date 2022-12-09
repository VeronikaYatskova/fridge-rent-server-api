using Fridge.Models.Requests;
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
        /// <param name="addRenterModel">Email and Password for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("renter/registration")]
        public async Task<IActionResult> RegisterRenter([FromBody] AddRenterModel addRenterModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = await authorizationService.RegisterRenter(addRenterModel);

                return Created("api/authorization/renter/register", token);
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
        /// Add new owner to the system.
        /// </summary>
        /// <param name="addOwnerModel">Name, Email, Password and Telephone for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/registration")]
        public async Task<IActionResult> RegisterOwner([FromBody] AddOwnerModel addOwnerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = await authorizationService.RegisterOwner(addOwnerModel);

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
        /// <param name="loginModel">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("renter/login")]
        public IActionResult LoginRenter([FromBody] LoginModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = authorizationService.LoginRenter(loginModel);

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
        /// <param name="loginModel">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/login")]
        public IActionResult LoginOwner([FromBody] LoginModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data");
                }

                var token = authorizationService.LoginOwner(loginModel);

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
