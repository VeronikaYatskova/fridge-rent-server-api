using Fridge.Models.DTOs.FridgeDto;
using Fridge.Models.DTOs.FridgeDtos;

namespace Fridge.Services.Abstracts
{
    public interface IRentService
    {
        Task<IEnumerable<FridgeDto>> GetRentersFridges();

        Task RentFridge(Guid fridgeId);

        Task Remove(Guid fridgeId);
    }
}
