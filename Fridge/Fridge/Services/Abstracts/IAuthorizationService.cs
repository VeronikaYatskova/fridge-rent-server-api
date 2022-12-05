using Fridge.Models.DTOs;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Models.DTOs.RenterDtos;

namespace Fridge.Services.Abstracts
{
    public interface IAuthorizationService
    {
        Task<string> RegisterRenter(RenterDto request);

        Task<string> RegisterOwner(OwnerDto request);

        string LoginOwner(LoginDto request);

        string LoginRenter(LoginDto loginDto);
    }
}
