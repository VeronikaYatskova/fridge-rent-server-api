using Fridge.Models.RoleBasedAuthorization;
using System.Linq.Expressions;

namespace Fridge.Repository.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);

        User FindByEmail(string email, bool trackChanges);

        void UpdateUser(User user);

        User FindByToken(string token, bool trackChanges);

        User FindUserByCondition(Expression<Func<User, bool>> condition, bool trackChanges);
    }
}
