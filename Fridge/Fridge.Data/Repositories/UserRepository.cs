using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Fridge.Data.Context;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Data.Models;

namespace Fridge.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public void AddRenter(User user) =>
            Create(user);

        public User? FindBy(Expression<Func<User, bool>> condition) =>
            FindByCondition(condition)?.FirstOrDefault();

        public void AddOwner(User owner) =>
            Create(owner);
    }
}
