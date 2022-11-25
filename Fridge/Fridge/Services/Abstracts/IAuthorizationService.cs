using Fridge.Models.DTOs;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Models.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Services.Abstracts
{
    public interface IAuthorizationService
    {
        Task<string> RegisterUser(UserDto request);

        Task<string> RegisterOwner(OwnerDto request);

        string LoginOwner(LoginDto request);

        string LoginUser(LoginDto loginDto);
    }
}
