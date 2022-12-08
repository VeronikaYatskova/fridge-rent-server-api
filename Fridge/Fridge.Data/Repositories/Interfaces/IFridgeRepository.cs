using Fridge.Data.Models;
using Fridge.Models.DTOs.FridgeDto;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IFridgeRepository
    {
        Task<IEnumerable<Models.Fridge>?> GetAvailableFridgesAsync();

        Task<IEnumerable<Models.Fridge>?> GetFridgesAsync();

        Task<Models.Fridge?> GetFridgeByIdAsync(Guid id);

        Task<IEnumerable<Models.Fridge>?> GetFridgeByConditionAsync(Expression<Func<Models.Fridge, bool>> expression);
       
        Guid AddFridge(FridgeServicePartDto data);

        void RemoveFridge(Models.Fridge data);

        void UpdateFridge(Models.Fridge fridge);
    }
}
