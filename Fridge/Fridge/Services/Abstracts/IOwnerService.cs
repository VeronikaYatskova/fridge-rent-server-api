using Fridge.Data.Models;
using Fridge.Models.DTOs;
using Fridge.Models.DTOs.OwnerDtos;
using Microsoft.AspNetCore.Mvc;
using Models.Models.DTOs;

namespace Fridge.Services.Abstracts
{
    public interface IOwnerService
    {
        Task<IEnumerable<FridgeDto>> GetOwnersFridges();

        Task<RentDocumentDto> GetRentedFridgeInfo(Guid fridgeId);

        Task<FridgeServiceDto> AddFridge(OwnerAddFridgeDto ownerAddFridgeDto);

        Task DeleteFridge(Guid fridgeId);
    }
}
