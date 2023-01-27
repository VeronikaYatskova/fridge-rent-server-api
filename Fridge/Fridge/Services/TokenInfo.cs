using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Fridge.Services
{
    public class TokenInfo
    {
        private readonly IHttpContextAccessor? httpContextAccessor;
        private readonly IRepositoryManager repository;
        private static IConfiguration configuration;

        public TokenInfo(IRepositoryManager repository, IHttpContextAccessor contextAccessor, IConfiguration config)
        {
            this.repository = repository;
            httpContextAccessor = contextAccessor;
            configuration = config;
        }

        public async Task<User?> GetUser()
        {
            var guid = await GetInfo();
            if (guid is not null)
            {
                var user = repository.User.FindBy(u => u.Id == Guid.Parse(guid));

                if (user is null)
                {
                    throw new ArgumentException("User is not found...");
                }

                return user;
            }

            return null;
        }

        public string CreateToken(User user)
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
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<string> UpdateRefreshToken(User user)
        {
            var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refresh-token"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                throw new ArgumentException("Invalid Refresh Token");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                throw new ArgumentException("Token is expired.");
            }

            string token = CreateToken(user);
            await SetRefreshToken(user);

            repository.User.UpdateUser(user);
            await repository.SaveAsync();

            return token;
        }

        public async Task SetRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            AppendTokenToCookies(refreshToken, user);
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7),
            };
        }

        private void AppendTokenToCookies(RefreshToken refreshToken, User user)
        {
            var cookiesOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };

            httpContextAccessor.HttpContext.Response.Cookies.Append("refresh-token", refreshToken.Token, cookiesOption);

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
        }

        private async Task<string?> GetInfo()
        {
            var guid = await GetUserGuid();

            return guid;
        }

        private async Task<string?> GetUserGuid()
        {
            var token = await httpContextAccessor?.HttpContext?.GetTokenAsync("access_token")!;

            if (token is not null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var userGuid = jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value;
                return userGuid;
            }

            return null;
        }
    }
}
