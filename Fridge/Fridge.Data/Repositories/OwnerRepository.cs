using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Fridge.Data.Context;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Data.Models;

namespace Fridge.Data.Repositories
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddOwner(Owner owner) =>
            Create(owner);

        public Owner? FindByEmail(string email, bool trackChanges) =>
            FindByCondition(o => o.Email == email, trackChanges)
            ?.FirstOrDefault();

        void Update(Owner owner) =>
            Update(owner);

        public async Task<Owner> GetOwnerByIdAsync(Guid id, bool trackChanges) =>
            await FindByCondition(o => o.Id == id, trackChanges)!
            .FirstOrDefaultAsync();

        public async Task<Owner> GetOwnerByConditionAsync(Expression<Func<Owner, bool>> condition, bool trackChanges) =>
            await FindByCondition(condition, trackChanges)
            .FirstOrDefaultAsync();
    }
}
