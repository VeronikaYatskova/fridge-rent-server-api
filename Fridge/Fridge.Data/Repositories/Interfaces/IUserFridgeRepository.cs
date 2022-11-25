
using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IUserFridgeRepository
    {
        void RentFridge(UserFridge userFridge);
        IEnumerable<UserFridge> GetUserFridge(Guid userId, bool trackChanges);
        UserFridge GetFridgeById(Guid fridgeId, bool trackChanges);
        UserFridge GetUserFridgeRow(Guid userId, Guid fridgeId, bool trackChanges);
        void RemoveFridge (UserFridge userFridge);
    }
}
