using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        void AddOwner(Owner owner);

        Owner? FindByEmail(string email, bool trackChanges);

        void Update(Owner owner);

        Task<Owner> GetOwnerByIdAsync(Guid id, bool trackChanges);

        Task<Owner> GetOwnerByConditionAsync(Expression<Func<Owner, bool>> condition, bool trackChanges);
    }
}
