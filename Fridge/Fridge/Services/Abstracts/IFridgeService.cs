using Fridge.Data.Models;
using Fridge.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Services.Abstracts
{
    public interface IFridgeService
    {
        Task<IEnumerable<FridgeDto>> GetFridges();

        Task<IEnumerable<Model>> GetModels();

        Task<IEnumerable<Producer>> GetProducers();
    }
}
