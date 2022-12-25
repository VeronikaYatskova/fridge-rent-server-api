using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);

        User? FindBy(Expression<Func<User, bool>> condition);

        void UpdateUser(User user);
    }
}
