using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;


namespace Fridge.Services
{
    public class TokenInfo
    {
        private static IHttpContextAccessor? httpContextAccessor;
        private readonly IRepositoryManager repository;

        public TokenInfo(IRepositoryManager repository, IHttpContextAccessor contextAccessor)
        {
            this.repository = repository;
            httpContextAccessor = contextAccessor;
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

        //public async Task<Owner?> GetOwner()
        //{
        //    var guid = await GetInfo();
        //    if (guid is not null)
        //    {
        //        var owner = repository.Owner.GetOwnerByConditionAsync(u => u.Id == Guid.Parse(guid)).Result;

        //        if (owner is null)
        //        {
        //            throw new ArgumentException("Owner is not found...");
        //        }

        //        return owner;
        //    }

        //    return null;
        //}

        private async static Task<string?> GetInfo()
        {
            var guid = await GetUserGuid();

            return guid;
        }

        private async static Task<string?> GetUserGuid()
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
