using Fridge.Models;
using System.Linq.Expressions;

namespace Repositories.Repository.Interfaces
{
    public interface IModelRepository
    {
        Model GetModelByIdAsync(Guid id, bool trackChanges);
        Task<Model> GetProducerByConditionAsync(Expression<Func<Model, bool>> condition, bool trackChanges);
        Task<IEnumerable<Model>> GetAllModels(bool trackChanges);
    }
}
