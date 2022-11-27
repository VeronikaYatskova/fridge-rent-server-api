using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Fridge.Controllers
{
    public class TokenInfo
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositoryManager _repository;

        public TokenInfo(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUser()
        {
            var guid = await GetInfo();
            var user = _repository.User.FindUserByCondition(u => u.Id == Guid.Parse(guid), trackChanges: false);

            if (user is null)
            {
                throw new ArgumentException("User are not found...");
            }

            return user;
        }

        public async Task<Owner> GetOwner()
        {
            var guid = await GetInfo();
            var owner = _repository.Owner.GetOwnerByConditionAsync(u => u.Id == Guid.Parse(guid), trackChanges: false).Result;

            if (owner is null)
            {
                throw new ArgumentException("Owner is not found...");
            }

            return owner;
        }

        private async static Task<string> GetInfo()
        {
            var guid = await GetUserGuid();

            return guid;
        }

        private async static Task<string> GetUserGuid()
        {
            var token = await _httpContextAccessor?.HttpContext?.GetTokenAsync("access_token")!;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userGuid = jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value;

            return userGuid;
        }
    }
}
