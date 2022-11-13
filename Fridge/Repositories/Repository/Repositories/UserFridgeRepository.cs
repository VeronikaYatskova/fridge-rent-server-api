using Fridge.Models;
using Fridge.Repository.Interfaces;

namespace Fridge.Repository.Repositories
{
    public class UserFridgeRepository : RepositoryBase<UserFridge>, IUserFridgeRepository
    {
        public UserFridgeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void RentFridge(UserFridge userFridge) =>
            Create(userFridge);

        public IEnumerable<UserFridge> GetUserFridge(Guid userId, bool trackChanges) =>
            FindByCondition(u => u.UserId == userId, trackChanges).ToList();

        public UserFridge? GetUserFridgeRow(Guid userId, Guid fridgeId, bool trackChanges) =>
            FindByCondition(u => u.UserId == userId && u.FridgeId == fridgeId, trackChanges)
            ?.FirstOrDefault();

        public UserFridge? GetFridgeById(Guid fridgeId, bool trackChanges) =>
            FindByCondition(u => u.FridgeId == fridgeId, trackChanges)
            ?.FirstOrDefault();

        public void RemoveFridge(UserFridge userFridge) =>
            Delete(userFridge);
    }
}
