using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        void AddOwner(Owner owner);

        Owner? FindByEmail(string email);

        Task<Owner> GetOwnerByIdAsync(Guid id);

        Task<Owner> GetOwnerByConditionAsync(Expression<Func<Owner, bool>> condition);
    }
}
