using Fridge.Data.Models;
using Fridge.Models.DTOs.FridgeDtos;

namespace Fridge.Services.Abstracts
{
    public interface IFridgeService
    {
        Task<IEnumerable<FridgeDto>> GetFridges();

        Task<IEnumerable<Model>> GetModels();

        Task<IEnumerable<Producer>> GetProducers();
    }
}
