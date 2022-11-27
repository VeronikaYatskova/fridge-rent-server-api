using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);

        User FindByEmail(string email, bool trackChanges);

        void UpdateUser(User user);

        User FindUserByCondition(Expression<Func<User, bool>> condition, bool trackChanges);
    }
}
