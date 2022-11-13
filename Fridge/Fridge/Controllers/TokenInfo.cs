using Fridge.Models.RoleBasedAuthorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Fridge.Controllers
{
    public class TokenInfo
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static string GetInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var guid = TokenInfo.GetUserGuid();

            return guid;
        }

        private static string GetUserGuid()
        {
            var token = _httpContextAccessor?.HttpContext?.GetTokenAsync("access_token").Result;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userGuid = jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value;

            return userGuid;
        }
    }
}
