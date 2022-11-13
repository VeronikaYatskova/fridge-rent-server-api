using Fridge.Models;
using System.Linq.Expressions;

namespace Fridge.Repository.Interfaces
{
    public interface IRentDocumentRepository
    {
        void AddDocument(RentDocument document);
        RentDocument FindDocumentByCondition(Expression<Func<RentDocument, bool>> expression, bool trackChanges);

    }
}
