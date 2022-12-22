using Fridge.Data.Models;
using Fridge.Models.Requests;
using Fridge.Models.Responses;

namespace Fridge.Services.Abstracts
{
    public interface IFridgeService
    {
        Task<IEnumerable<FridgeModel>> GetFridges();

        Task<IEnumerable<FridgeModelModel>> GetModels();

        Task<IEnumerable<FridgeProducerModel>> GetProducers();

        Task<IEnumerable<IUserFridgeModel>> GetUserFridges();

        Task AddFridge(AddFridgeOwnerModel addFridgeOwnerModel);

        Task RentFridge(Guid fridgeId);

        Task DeleteFridge(Guid fridgeId);

        Task ReturnFridge(Guid fridgeId);
    }
}
