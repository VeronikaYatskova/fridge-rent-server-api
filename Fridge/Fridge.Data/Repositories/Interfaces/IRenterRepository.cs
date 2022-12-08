using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IRenterRepository
    {
        void AddRenter(Renter user);

        Renter? FindByEmail(string email);

        Renter? GetRenterById(Guid? renterId);

        Renter? FindRenterByCondition(Expression<Func<Renter, bool>> condition);
    }
}
