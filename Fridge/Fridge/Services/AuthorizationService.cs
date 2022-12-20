using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.Requests;
using Fridge.Services.Abstracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;


namespace Fridge.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepositoryManager repository;
        private readonly ILogger<AuthorizationService> logger;
        private readonly IConfiguration configuration;

        public AuthorizationService(IConfiguration configuration, IRepositoryManager repository, ILogger<AuthorizationService> logger)
        {
            this.repository = repository;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<string> RegisterUser(AddUserModel addUserModel, string role)
        {
            if (repository.User.FindBy(u => u.Email == addUserModel.Email && u.Role == role) is not null)
            {
                logger.LogInformation($"{role} with the same email is already in the database.");
                throw new ArgumentException($"{role} with the same email has been registered.");
            }

            CreatePasswordHash(addUserModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var renter = new User
            {
                Id = Guid.NewGuid(),
                Email = addUserModel.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            string token = CreateToken(renter);

            repository.User.AddRenter(renter);
            await repository.SaveAsync();

            return token;
        }

        public string LoginUser(LoginModel loginModel, string role)
        {
            var renter = repository.User.FindBy(u => u.Email == loginModel.Email && u.Role == role);

            if (renter is null)
            {
                throw new ArgumentException($"{role} is not found");
            }

            VerifyData(renter, loginModel);

            string token = CreateToken(renter);

            return token;
        }

        private void VerifyData(User user, LoginModel loginModel)
        {
            if (user!.Email != loginModel.Email)
            {
                throw new ArgumentException("Not found.");
            }

            if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                throw new ArgumentException("Wrong password");
            }
        }

        private string CreateToken(User user)
        {
            var userId = user.Id;
            var claims = new[]
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
