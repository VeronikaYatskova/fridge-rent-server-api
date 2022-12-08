using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IModelRepository
    {
        Model GetModelByIdAsync(Guid id);
        Task<IEnumerable<Model>> GetAllModels();
    }
}
