using Fridge.Models;
using Fridge.Models.RoleBasedAuthorization;
using Fridge.Repository;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interfaces;
using System.Linq.Expressions;

namespace Repositories.Repository.Repositories
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
