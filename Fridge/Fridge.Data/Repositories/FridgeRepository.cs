using Fridge.Data.Context;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs.FridgeDto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories
{
    public class FridgeRepository : RepositoryBase<Models.Fridge>, IFridgeRepository
    {
        public FridgeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Models.Fridge>?> GetAllFridgesAsync(bool trackChanges) =>
            await FindByCondition(f => f.IsRented == false, trackChanges)!
            .OrderBy(f => f.Id)
            .ToListAsync();

        public async Task<IEnumerable<Models.Fridge>?> GetFridgesAsync(bool trackChanges) =>
            await FindAll(trackChanges)!
            .ToListAsync();

        public async Task<Models.Fridge?> GetFridgeByIdAsync(Guid id, bool trackChanges) =>
            await FindByCondition(f => f.Id == id, trackChanges)!
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Models.Fridge>?> GetFridgeByConditionAsync(Expression<Func<Models.Fridge, bool>> expression, bool trackChanges) =>
            await FindByCondition(expression, trackChanges)!
            .ToListAsync();

        public Guid AddFridge(FridgeServicePartDto data)
        {
            var newFridge = new Models.Fridge
            {
                Id = data.FridgeId,
                ModelId = data.ModelId,
                OwnerId = data.OwnerId,
                ProducerId = data.ProducerId,
                Capacity = data.Capacity,
                IsRented = false,
            };

            Create(newFridge);

            return newFridge.Id;
        }

        public void UpdateFridge(Models.Fridge fridge) =>
            Update(fridge);

        public void RemoveFridge(Models.Fridge data) =>
            Delete(data);
    }
}
