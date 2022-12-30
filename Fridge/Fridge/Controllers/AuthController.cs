using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
        /// Add new user.
        /// </summary>
        /// <param name="addUserModel">Email, Password for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("sign-up")]
        public async Task<IActionResult> RegisterUser([FromBody] AddUserModel addUserModel)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var userRole = addUserModel.IsOwner ? UserRoles.Owner : UserRoles.Renter;

            var token = await authorizationService.RegisterUser(addUserModel, userRole);

            return Created("", token);
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="loginModel">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var token = await authorizationService.LoginUser(loginModel);

            return Created("", token);
        }

        /// <summary>
        /// Refresh token.
        /// </summary>
        /// <returns>New token.</returns>
        [HttpPost("refresh")]
        [Authorize(Roles = $"{UserRoles.Owner}, {UserRoles.Renter}")]
        public async Task<ActionResult<string>> RefreshToken() =>
               await authorizationService.GetRefreshToken();
    }
}
