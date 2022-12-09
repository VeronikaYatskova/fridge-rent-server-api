using Fridge.Models.Requests;

namespace Fridge.Services.Abstracts
{
    public interface IAuthorizationService
    {
        Task<string> RegisterRenter(AddRenterModel request);

        Task<string> RegisterOwner(AddOwnerModel request);

        string LoginOwner(LoginModel request);

        string LoginRenter(LoginModel loginDto);
    }
}
