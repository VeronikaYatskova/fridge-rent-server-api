using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IModelRepository
    {
        Task<IEnumerable<Model>> GetAllModels();
    }
}
