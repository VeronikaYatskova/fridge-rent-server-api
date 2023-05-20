using Fridge.Models.Requests;

namespace Fridge.Services.Abstracts
{
    public interface IAuthService
    {
        Task<string> RegisterUser(AddUserModel addUserModel);

        Task<string> RegisterThroughSocialMedia(AddUserSocialAuth addUserSocialAuthModel);

        Task<string> LoginUser(LoginModel loginModel);

        Task<string> GetRefreshToken();
    }
}
