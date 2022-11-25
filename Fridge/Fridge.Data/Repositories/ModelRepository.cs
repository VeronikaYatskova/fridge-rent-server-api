using Microsoft.EntityFrameworkCore;
using Fridge.Data.Context;
using Fridge.Data.Repositories.Interfaces;
using System.Linq.Expressions;
using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public class ModelRepository : RepositoryBase<Model>, IModelRepository
    {
        public ModelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public Model GetModelByIdAsync(Guid id, bool trackChanges) =>
             FindByCondition(m => m.Id == id, trackChanges)!
            .FirstOrDefault();

        public async Task<Model> GetProducerByConditionAsync(Expression<Func<Model, bool>> condition, bool trackChanges) =>
            await FindByCondition(condition, trackChanges)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Model>> GetAllModels(bool trackChanges) =>
            await FindAll(trackChanges)
            .ToListAsync();
    }
}
