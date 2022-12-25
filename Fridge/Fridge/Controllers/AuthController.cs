using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Services;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authorizationService;

        public AuthController(IAuthService service)
        {
            this.authorizationService = service;
        }

        /// <summary>
        /// Add new owner to the system.
        /// </summary>
        /// <param name="addUserModel">Name, Email, Password and Telephone for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("sign-up")]
        public async Task<IActionResult> RegisterUser([FromBody] AddUserModel addUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }

                var userRole = addUserModel.IsOwner ? UserRoles.Owner : UserRoles.Renter;

                var token = await authorizationService.RegisterUser(addUserModel, userRole);
                
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
        /// Login to the system as an owner.
        /// </summary>
        /// <param name="loginModel">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }

                var token = await authorizationService.LoginUser(loginModel);

                return Created("api/auth/owner/sign-in", token);
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

        [HttpPost("refresh-token")]
        [Authorize(Roles = $"{UserRoles.Owner}, {UserRoles.Renter}")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            try
            {
                return await authorizationService.GetRefreshToken();
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
