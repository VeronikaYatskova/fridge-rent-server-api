using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void AddRenter(User user);

        User? FindBy(Expression<Func<User, bool>> condition);

        void AddOwner(User owner);
    }
}
