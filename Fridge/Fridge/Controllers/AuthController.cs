using Fridge.Models.DTOs;
using Fridge.Models.RoleBasedAuthorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Models.RoleBasedAuthorization;
using Repositories.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/authorization")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IRepositoryManager _repository;
        private readonly ILogger<AuthController> _logger;
        public static User? user;
        public static Owner? owner;

        public AuthController(IConfiguration configuration, IRepositoryManager _repository, ILogger<AuthController> logger)
        {
            this.configuration = configuration;
            this._repository = _repository;
            _logger = logger;
        }

        /// <summary>
        /// Add new user to the system.
        /// </summary>
        /// <param name="request">Email and Password for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("user/register")]
        public async Task<ActionResult<string>> RegisterAsUser(UserDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserDto object");
                return UnprocessableEntity(ModelState);
            }

            if (_repository.User.FindByEmail(request.Email, trackChanges:false) is not null)
            {
                _logger.LogInformation($"User with the same email is already in the database.");
                return BadRequest("User with the same email is already in the database.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = request.Role
            };

            _repository.User.AddUser(user);
            await _repository.SaveAsync();

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);

            _repository.User.UpdateUser(user);
            await _repository.SaveAsync();

            return Created("api/authorization/user/register", token);
        }
        
        /// <summary>
        /// Add new owner to the system.
        /// </summary>
        /// <param name="request">Name, Email, Password and Telephone for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/register")]
        public async Task<ActionResult<string>> RegisterAsOwner([FromBody] OwnerDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the OwnerDto object");
                return UnprocessableEntity(ModelState);
            }

            if (_repository.Owner.FindByEmail(request.Email, trackChanges: false) is not null)
            {
                _logger.LogInformation($"Owner with the same email is already in the database.");
                return BadRequest("Owner with the same email is already in the database.");
            }
            
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            owner = new Owner
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            _repository.Owner.AddOwner(owner);
            await _repository.SaveAsync();

            string token = CreateToken(owner);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, owner);

            _repository.Owner.Update(owner);
            await _repository.SaveAsync();

            return Created("api/authorization/owner/register", token);
        }

        /// <summary>
        /// Login to the system as an owner.
        /// </summary>
        /// <param name="request">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("owner/login")]
        public async Task<ActionResult<string>> LoginAsOwner(UserLoginDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserLoginDto object");
                return UnprocessableEntity(ModelState);
            }

            if (owner is not null) owner = null;
            
            owner ??= _repository.Owner.FindByEmail(request.Email, trackChanges: false);

            if (owner!.Email != request.Email)
            {
                return NotFound("Owner not found.");
            }

            if (!VerifyPasswordHash(request.Password, owner.PasswordHash!, owner.PasswordSalt!))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(owner);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, owner);

            _repository.Owner.Update(owner);
            await _repository.SaveAsync();

            return Created("api/authorization/owner/login", token);
        }

        /// <summary>
        /// Login to the system as a user.
        /// </summary>
        /// <param name="request">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("user/login")]
        public async Task<ActionResult<string>> LoginAsUser(UserLoginDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the UserLoginDto object");
                return UnprocessableEntity(ModelState);
            }

            if (user is not null) user = null;
            
            user ??= _repository.User.FindByEmail(request.Email, trackChanges: false);

            if (user is null)
            {
                return NotFound("User not found.");
            }

            if (user.Email != request.Email)
            {
                return NotFound("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);

            _repository.User.UpdateUser(user);
            await _repository.SaveAsync();

            return Created("api/authorization/user/login", token);
        }

        [HttpPost("user/refresh-token")]
        public async Task<ActionResult<string>> RefreshUsersToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!_repository.User.FindByToken(refreshToken, trackChanges:false).Token.Equals(refreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }
            else if (user.Expires < DateTime.Now)
            {
                return Unauthorized("Token expires");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            return Ok(token);
        }

        [HttpPost("owner/refresh-token")]
        public async Task<ActionResult<string>> RefreshOwnersToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!_repository.Owner.FindByToken(refreshToken, trackChanges:false).Token.Equals(refreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }
            else if (owner.Expires < DateTime.Now)
            {
                return Unauthorized("Token expires");
            }

            string token = CreateToken(owner);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, owner);

            return Ok(token);
        }

        private string CreateToken(IUser user)
        {
            var userId = user.Id;
            var claims = new []
            {
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, IUser user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.Token = newRefreshToken.Token;
            user.Created = newRefreshToken.Created;
            user.Expires = newRefreshToken.Expires;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
