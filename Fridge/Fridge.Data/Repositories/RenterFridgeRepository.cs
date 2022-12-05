using Fridge.Data.Context;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;

namespace Fridge.Data.Repositories
{
    public class RenterFridgeRepository : RepositoryBase<RenterFridge>, IRenterFridgeRepository
    {
        public RenterFridgeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void RentFridge(RenterFridge userFridge) =>
            Create(userFridge);

        public IEnumerable<RenterFridge> GetRenterFridge(Guid userId, bool trackChanges) =>
            FindByCondition(u => u.RenterId == userId, trackChanges).ToList();

        public RenterFridge? GetRenterFridgeRow(Guid userId, Guid fridgeId, bool trackChanges) =>
            FindByCondition(u => u.RenterId == userId && u.FridgeId == fridgeId, trackChanges)
            ?.FirstOrDefault();

        public RenterFridge? GetFridgeById(Guid fridgeId, bool trackChanges) =>
            FindByCondition(u => u.FridgeId == fridgeId, trackChanges)
            ?.FirstOrDefault();

        public void RemoveFridge(RenterFridge userFridge) =>
            Delete(userFridge);
    }
}
