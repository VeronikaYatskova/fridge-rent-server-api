using Fridge.Models.Requests;

namespace Fridge.Services.Abstracts
{
    public interface IAuthService
    {
        Task<string> RegisterUser(AddUserModel addUserModel, string role);

        Task<string> LoginUser(LoginModel loginModel);

        Task<string> GetRefreshToken();
    }
}
