using Fridge.Data.Models;
using Fridge.Data.Models.Abstracts;
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

        public async Task<string> RegisterRenter(AddRenterModel renterDto)
        {
            if (repository.Renter.FindByEmail(renterDto.Email) is not null)
            {
                logger.LogInformation($"Renter with the same email is already in the database.");
                throw new ArgumentException("Renter with the same email has been registered.");
            }

            CreatePasswordHash(renterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var renter = new Renter
            {
                Id = Guid.NewGuid(),
                Email = renterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = renterDto.Role
            };

            string token = CreateToken(renter);

            repository.Renter.AddRenter(renter);
            await repository.SaveAsync();

            return token;
        }

        public async Task<string> RegisterOwner(AddOwnerModel ownerDto)
        {
            if (repository.Owner.FindByEmail(ownerDto.Email) is not null)
            {
                logger.LogInformation($"Owner with the same email is already in the database.");
                throw new ArgumentException("Owner with the same email has been registered.");
            }

            CreatePasswordHash(ownerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var owner = new Owner
            {
                Id = Guid.NewGuid(),
                Name = ownerDto.Name,
                Email = ownerDto.Email,
                Phone = ownerDto.Phone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            repository.Owner.AddOwner(owner);
            await repository.SaveAsync();

            string token = CreateToken(owner);

            return token;
        }

        public string LoginRenter(LoginModel loginDto)
        {
            var renter = repository.Renter.FindByEmail(loginDto.Email);

            if (renter is null)
            {
                throw new ArgumentException("User is not found");
            }

            VerifyData(renter, loginDto);

            string token = CreateToken(renter);

            return token;
        }

        public string LoginOwner(LoginModel loginDto)
        {
            var owner = repository.Owner.FindByEmail(loginDto.Email);

            if (owner is null)
            {
                throw new ArgumentException("User is not found");
            }

            VerifyData(owner, loginDto);

            string token = CreateToken(owner);

            return token;
        }

        private void VerifyData(IUser user, LoginModel loginDto)
        {
            if (user!.Email != loginDto.Email)
            {
                throw new ArgumentException("Not found.");
            }

            if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                throw new ArgumentException("Wrong password");
            }
        }

        private string CreateToken(IUser user)
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
