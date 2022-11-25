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
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthController> _logger;

        private readonly IAuthorizationService authorizationService;

        public AuthController(IConfiguration configuration, IAuthorizationService service, ILogger<AuthController> logger)
        {
            this.configuration = configuration;
            this.authorizationService = service;
            this._logger = logger;
        }

        /// <summary>
        /// Add new user to the system.
        /// </summary>
        /// <param name="userDto">Email and Password for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("user/register")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserDto object");
                return UnprocessableEntity(ModelState);
            }

            var token = await authorizationService.RegisterUser(userDto);

            return Created("api/authorization/user/register", token);
        }

        /// <summary>
        /// Add new owner to the system.
        /// </summary>
        /// <param name="ownerDto">Name, Email, Password and Telephone for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/register")]
        public async Task<ActionResult<string>> RegisterOwner([FromBody] OwnerDto ownerDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the OwnerDto object");
                return UnprocessableEntity(ModelState);
            }

            var token = await authorizationService.RegisterOwner(ownerDto);

            return Created("api/authorization/owner/register", token);
        }

        /// <summary>
        /// Login to the system as an owner.
        /// </summary>
        /// <param name="loginDto">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/login")]
        public ActionResult<string> LoginOwner([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserLoginDto object");
                return UnprocessableEntity(ModelState);
            }

            var token = authorizationService.LoginOwner(loginDto);
            
            return Created("api/authorization/owner/login", token);
        }

        /// <summary>
        /// Login to the system as a user.
        /// </summary>
        /// <param name="loginDto">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("user/login")]
        public ActionResult<string> LoginUser([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserLoginDto object");
                return UnprocessableEntity(ModelState);
            }

            var token = authorizationService.LoginUser(loginDto);
            
            return Created("api/authorization/user/login", token);
        }
    }
}
