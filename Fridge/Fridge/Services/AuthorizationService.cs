using Fridge.Data.Models;
using Fridge.Data.Models.Abstracts;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Models.DTOs.UserDtos;
using Fridge.Services.Abstracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Fridge.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly IConfiguration configuration;

        public AuthorizationService(IConfiguration configuration, IRepositoryManager repository, ILogger<AuthorizationService> logger)
        {
            _repository = repository;
            _logger = logger;
            this.configuration = configuration;
        }

        public async Task<string> RegisterUser(UserDto userDto)
        {
            if (_repository.User.FindByEmail(userDto.Email, trackChanges: false) is not null)
            {
                _logger.LogInformation($"User with the same email is already in the database.");
                throw new ArgumentException("User with the same email has been registered.");
            }

            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = userDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = userDto.Role
            };

            string token = CreateToken(user);

            _repository.User.AddUser(user);
            await _repository.SaveAsync();

            return token;
        }

        public async Task<string> RegisterOwner(OwnerDto ownerDto)
        {
            if (_repository.Owner.FindByEmail(ownerDto.Email, trackChanges: false) is not null)
            {
                _logger.LogInformation($"Owner with the same email is already in the database.");
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

            _repository.Owner.AddOwner(owner);
            await _repository.SaveAsync();

            string token = CreateToken(owner);

            return token;
        }

        public string LoginUser(LoginDto loginDto)
        {
            var user = _repository.User.FindByEmail(loginDto.Email, trackChanges: false);

            VerifyData(user, loginDto);

            string token = CreateToken(user);

            return token;
        }

        public string LoginOwner(LoginDto loginDto)
        {
            var owner = _repository.Owner.FindByEmail(loginDto.Email, trackChanges: false);

            VerifyData(owner, loginDto);

            string token = CreateToken(owner);

            return token;
        }

        private void VerifyData(IUser user, LoginDto loginDto)
        {
            if (user!.Email != loginDto.Email)
            {
                throw new Exception("Not found.");
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
