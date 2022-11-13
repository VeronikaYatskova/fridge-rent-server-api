using Fridge.Models;
using System.Linq.Expressions;

namespace Repositories.Repository.Interfaces
{
    public interface IProducerRepository
    {
        Task<Producer> GetProducerByIdAsync(Guid id, bool trackChanges);

        Task<Producer> GetProducerByConditionAsync(Expression<Func<Producer, bool>> condition, bool trackChanges);

        Task<IEnumerable<Producer>> GetAllProducers(bool trackChanges);
    }
}
