using Castle.Core.Resource;
using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Models.Responses;
using Fridge.Services.Abstracts;
using Fridge.Utils.Filters;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [ValidationFilter]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authorizationService;
        private readonly IConfiguration configuration;

        public object GoogleScope { get; private set; }

        public AuthController(IAuthService service, IConfiguration configuration)
        {
            this.authorizationService = service;
            this.configuration = configuration;
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="addUserModel">Email, Password for registration.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("sign-up")]
        public async Task<IActionResult> RegisterUser([FromBody] AddUserModel addUserModel)
        {
            var token = await authorizationService.RegisterUser(addUserModel);

            return Created("", token);
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="loginModel">Email and Password for Logging In.</param>
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel loginModel)
        {
            var token = await authorizationService.LoginUser(loginModel);

            return Created("", token);
        }

        /// <summary>
        /// Refresh token.
        /// </summary>
        /// <returns>New token.</returns>
        [HttpPost("refresh")]
        [Authorize(Roles = $"{UserRoles.Owner}, {UserRoles.Renter}")]
        public async Task<ActionResult<string>> RefreshToken() =>
               await authorizationService.GetRefreshToken();

        [HttpPost("google/sing-up")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GoogleAuth([FromBody] ExternalAuthDataRequest authorizationData)
        {
            var token = await ExchangeGoogleCodeForAccessToken(authorizationData);

            var user = await GetUserInfoByToken(token.AccessToken);

            var userGoogleAuthData = new AddUserSocialAuth()
            {
                SocialId = user.Id,
                AuthVia = SocialMedia.Google,
                IsOwner = authorizationData.IsOwner,
            };

            var accessToken = await authorizationService.RegisterThroughSocialMedia(userGoogleAuthData);

            return Ok(accessToken);
        }

        [HttpPost("vk/sign-up")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> VkAuth([FromBody] ExternalAuthDataRequest authorizationData)
        {
            var user = await GetUserInfoByVkCode(authorizationData.Code);

            var userVkAuthData = new AddUserSocialAuth()
            {
                SocialId = user.User_Id,
                AuthVia = SocialMedia.VK,
                IsOwner = authorizationData.IsOwner,
            };

            var accessToken = await authorizationService.RegisterThroughSocialMedia(userVkAuthData);

            return Ok(accessToken);
        }

        private async Task<TokenResponse> ExchangeGoogleCodeForAccessToken(ExternalAuthDataRequest authorizationData)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = configuration["GoogleAuth:ClientId"],
                ClientSecret = configuration["GoogleAuth:ClientSecret"]
            };

            var credential = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets
                });

            TokenResponse token = await credential.ExchangeCodeForTokenAsync(
                "",
                authorizationData.Code,
                "http://localhost:3000/auth/google/sign-in",
                CancellationToken.None
            );

            return token;
        }

        private async Task<GoogleDataResponse> GetUserInfoByToken(string token)
        {
            HttpClient client = new HttpClient();

            string cliUrl = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={token}";

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, cliUrl);
            using HttpResponseMessage response = await client.SendAsync(request);

            var jsonUserData = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<GoogleDataResponse>(jsonUserData);

            return user;
        }

        private async Task<VkDataResponse> GetUserInfoByVkCode(string code)
        {
            HttpClient client = new HttpClient();

            var clientId = configuration["VkAuth:ClientId"];
            var clientSecret = configuration["VkAuth:ClientSecret"];
            var redirectUri = "http://localhost:3000/auth/vk/sign-in";

            string cliUrl = $"https://oauth.vk.com/access_token?client_id={clientId}&client_secret={clientSecret}&redirect_uri={redirectUri}&code={code}";

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, cliUrl);
            using HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonUserData = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<VkDataResponse>(jsonUserData);

                return user;
            }
            else
            {
                throw new ArgumentException("Bad parameters...");
            }
        }
    }
}
