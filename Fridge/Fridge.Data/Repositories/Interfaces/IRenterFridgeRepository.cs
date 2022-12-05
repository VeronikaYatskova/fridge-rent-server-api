
using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IRenterFridgeRepository
    {
        void RentFridge(RenterFridge userFridge);
        IEnumerable<RenterFridge> GetRenterFridge(Guid userId, bool trackChanges);
        RenterFridge GetFridgeById(Guid fridgeId, bool trackChanges);
        RenterFridge GetRenterFridgeRow(Guid userId, Guid fridgeId, bool trackChanges);
        void RemoveFridge (RenterFridge userFridge);
    }
}
