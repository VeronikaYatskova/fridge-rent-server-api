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

        public Owner? FindByEmail(string email) =>
            FindByCondition(o => o.Email == email)
            ?.FirstOrDefault();

        public async Task<Owner?> GetOwnerByIdAsync(Guid id) =>
            await FindByCondition(o => o.Id == id)
            .FirstOrDefaultAsync();

        public async Task<Owner?> GetOwnerByConditionAsync(Expression<Func<Owner, bool>> condition) =>
            await FindByCondition(condition)
            .FirstOrDefaultAsync();
    }
}
