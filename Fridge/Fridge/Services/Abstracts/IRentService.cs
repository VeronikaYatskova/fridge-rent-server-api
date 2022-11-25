using Fridge.Models.DTOs;

namespace Fridge.Services.Abstracts
{
    public interface IRentService
    {
        Task<IEnumerable<FridgeDto>> GetUsersFridges();

        Task RentFridge(Guid fridgeId);

        Task Remove(Guid fridgeId);
    }
}
