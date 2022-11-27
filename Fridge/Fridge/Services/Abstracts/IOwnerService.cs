using Fridge.Models.DTOs.FridgeDto;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Models.DTOs.OwnerDtos;

namespace Fridge.Services.Abstracts
{
    public interface IOwnerService
    {
        Task<IEnumerable<FridgeDto>> GetOwnersFridges();

        Task<RentDocumentDto> GetRentedFridgeInfo(Guid fridgeId);

        Task<FridgeServicePartDto> AddFridge(OwnerAddFridgeDto ownerAddFridgeDto);

        Task DeleteFridge(Guid fridgeId);
    }
}
