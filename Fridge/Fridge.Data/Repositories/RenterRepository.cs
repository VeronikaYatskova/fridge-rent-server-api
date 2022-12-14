using System.Linq.Expressions;
using Fridge.Data.Context;
using Fridge.Data.Models;

namespace Fridge.Data.Repositories.Interfaces
{
    public class RenterRepository : RepositoryBase<Renter>, IRenterRepository
    {
        public RenterRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddRenter(Renter user) =>
            Create(user);

        public Renter? FindByEmail(string email) =>
            FindByCondition(u => u.Email == email)?.FirstOrDefault();

        public Renter? FindRenterByCondition(Expression<Func<Renter, bool>> condition) =>
            FindByCondition(condition)?.FirstOrDefault();
    }
}
