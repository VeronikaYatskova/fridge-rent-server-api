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

        public Renter FindByEmail(string email, bool trackChanges) =>
            FindByCondition(u => u.Email == email, trackChanges).FirstOrDefault();

        public void UpdateRenter(Renter user) =>
            Update(user);

        public Renter FindRenterByCondition(Expression<Func<Renter, bool>> condition, bool trackChanges) =>
            FindByCondition(condition, trackChanges).FirstOrDefault();
    }
}
