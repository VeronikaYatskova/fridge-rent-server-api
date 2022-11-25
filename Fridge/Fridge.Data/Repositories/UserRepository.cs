using System.Linq.Expressions;
using Fridge.Data.Context;
using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddUser(User user) =>
            Create(user);

        public User FindByEmail(string email, bool trackChanges) =>
            FindByCondition(u => u.Email == email, trackChanges).FirstOrDefault();

        public void UpdateUser(User user) =>
            Update(user);

        public User FindUserByCondition(Expression<Func<User, bool>> condition, bool trackChanges) =>
            FindByCondition(condition, trackChanges).FirstOrDefault();
    }
}
