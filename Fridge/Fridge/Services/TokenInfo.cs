using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Fridge.Services
{
    public class TokenInfo
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private readonly IRepositoryManager _repository;
        private bool isFounded = false;

        public TokenInfo(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Renter?> GetUser()
        {
            var guid = await GetInfo();
            if (guid is not null)
            {
                var user = _repository.Renter.FindRenterByCondition(u => u.Id == Guid.Parse(guid));

                if (user is null)
                {
                    throw new ArgumentException("Renter is not found...");
                }

                return user;
            }

            return null;
        }

        public async Task<Owner?> GetOwner()
        {
            var guid = await GetInfo();
            if (guid is not null)
            {
                var owner = _repository.Owner.GetOwnerByConditionAsync(u => u.Id == Guid.Parse(guid)).Result;

                if (owner is null)
                {
                    throw new ArgumentException("Owner is not found...");
                }

                return owner;
            }

            return null;
        }

        private async static Task<string?> GetInfo()
        {
            var guid = await GetUserGuid();

            return guid;
        }

        private async static Task<string?> GetUserGuid()
        {
            var token = await _httpContextAccessor?.HttpContext?.GetTokenAsync("access_token")!;

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
