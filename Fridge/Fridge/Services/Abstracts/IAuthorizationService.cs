using Fridge.Models.Requests;

namespace Fridge.Services.Abstracts
{
    public interface IAuthorizationService
    {
        Task<string> RegisterUser(AddUserModel addUserModel, string role);

        string LoginUser(LoginModel loginModel, string role);
    }
}
