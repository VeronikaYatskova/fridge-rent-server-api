using Fridge.Data.Models;
using Fridge.Models.Requests;
using Fridge.Models.Responses;

namespace Fridge.Services.Abstracts
{
    public interface IFridgeService
    {
        Task<IEnumerable<FridgeModel>> GetFridges();

        Task<IEnumerable<Model>> GetModels();

        Task<IEnumerable<Producer>> GetProducers();

        Task<IEnumerable<FridgeModel>> GetOwnersFridges();

        Task<RentDocumentModel> GetRentedFridgeInfo(Guid fridgeId);

        Task<IEnumerable<FridgeModel>> GetRentersFridges();

        Task AddFridge(AddFridgeOwnerModel addFridgeOwnerModel);

        Task RentFridge(Guid fridgeId);

        Task DeleteFridge(Guid fridgeId);

        Task ReturnFridge(Guid fridgeId);
    }
}
