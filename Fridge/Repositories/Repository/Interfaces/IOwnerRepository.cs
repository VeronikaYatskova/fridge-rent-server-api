using Fridge.Models.RoleBasedAuthorization;
using System.Linq.Expressions;

namespace Fridge.Repository.Interfaces
{
    public interface IOwnerRepository
    {
        void AddOwner(Owner owner);

        Owner? FindByEmail(string email, bool trackChanges);

        void Update(Owner owner);

        Owner FindByToken(string token, bool trackChanges);

        Task<Owner> GetOwnerByIdAsync(Guid id, bool trackChanges);

        Task<Owner> GetOwnerByConditionAsync(Expression<Func<Owner, bool>> condition, bool trackChanges);
    }
}
