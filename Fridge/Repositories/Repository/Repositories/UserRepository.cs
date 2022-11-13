using Fridge.Models.RoleBasedAuthorization;
using Fridge.Repository.Interfaces;
using System.Linq.Expressions;

namespace Fridge.Repository.Repositories
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

        public User FindByToken(string token, bool trackChanges) =>
            FindByCondition(u => u.Token == token, trackChanges).FirstOrDefault();

        public User FindUserByCondition(Expression<Func<User, bool>> condition, bool trackChanges) =>
            FindByCondition(condition, trackChanges).FirstOrDefault();
    }
}
