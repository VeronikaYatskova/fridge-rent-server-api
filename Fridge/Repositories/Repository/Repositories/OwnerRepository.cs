using Fridge.Models;
using Fridge.Models.RoleBasedAuthorization;
using Fridge.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fridge.Repository.Repositories
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

        public Owner FindByToken(string token, bool trackChanges) =>
            FindByCondition(o => o.Token == token, trackChanges)
            .FirstOrDefault();

        public async Task<Owner> GetOwnerByIdAsync(Guid id, bool trackChanges) =>
            await FindByCondition(o => o.Id == id, trackChanges)!
            .FirstOrDefaultAsync();

        public async Task<Owner> GetOwnerByConditionAsync(Expression<Func<Owner, bool>> condition, bool trackChanges) =>
            await FindByCondition(condition, trackChanges)
            .FirstOrDefaultAsync();
    }
}
