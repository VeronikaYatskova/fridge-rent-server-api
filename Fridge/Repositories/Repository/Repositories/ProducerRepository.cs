using Fridge.Models;
using Fridge.Repository;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interfaces;
using System.Linq.Expressions;

namespace Repositories.Repository.Repositories
{
    public class ProducerRepository : RepositoryBase<Producer>, IProducerRepository
    {
        public ProducerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Producer> GetProducerByConditionAsync(Expression<Func<Producer, bool>> condition, bool trackChanges) =>
            await FindByCondition(condition, trackChanges)
            .FirstOrDefaultAsync();

        public async Task<Producer> GetProducerByIdAsync(Guid id, bool trackChanges) =>
            await FindByCondition(p => p.Id == id, trackChanges)!
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Producer>> GetAllProducers(bool trackChanges) =>
            await FindAll(trackChanges)
            .ToListAsync();
    }
}
