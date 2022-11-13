using Models.Models.DTOs;
using System.Linq.Expressions;

namespace Fridge.Repository.Interfaces
{
    public interface IFridgeRepository
    {
        Task<IEnumerable<Models.Fridge>?> GetAllFridgesAsync(bool trackChanges);

        Task<IEnumerable<Models.Fridge>?> GetFridgesAsync(bool trackChanges);

        Task<Models.Fridge?> GetFridgeByIdAsync(Guid id, bool trackChanges);

        Task<IEnumerable<Models.Fridge>?> GetFridgeByConditionAsync(Expression<Func<Models.Fridge, bool>> expression, bool trackChanges);
       
        Guid AddFridge(FridgeServiceDto data);

        void RemoveFridge(Models.Fridge data);

        void UpdateFridge(Models.Fridge fridge);
    }
}
