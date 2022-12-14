using Fridge.Data.Models;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories.Interfaces
{
    public interface IRentDocumentRepository
    {
        void AddDocument(RentDocument document);
        void RemoveDocument(RentDocument document);
    }
}
