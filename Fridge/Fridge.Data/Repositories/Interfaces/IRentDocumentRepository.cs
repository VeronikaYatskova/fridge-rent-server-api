using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IRentDocumentRepository
    {
        void AddDocument(RentDocument document);
        RentDocument? FindDocumentByCondition(Expression<Func<RentDocument, bool>> expression);
        void RemoveDocument(RentDocument document);
    }
}
