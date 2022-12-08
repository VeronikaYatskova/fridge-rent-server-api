using Microsoft.EntityFrameworkCore;
using Fridge.Data.Context;
using Fridge.Data.Models;


namespace Fridge.Data.Repositories.Interfaces
{
    public class ModelRepository : RepositoryBase<Model>, IModelRepository
    {
        public ModelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public Model GetModelByIdAsync(Guid id) =>
             FindByCondition(m => m.Id == id)!
            .FirstOrDefault()!;

        public async Task<IEnumerable<Model>> GetAllModels() =>
            await FindAll()!
            .ToListAsync();
    }
}
