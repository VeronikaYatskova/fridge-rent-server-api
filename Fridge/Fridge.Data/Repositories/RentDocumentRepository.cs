using Fridge.Data.Context;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Fridge.Data.Repositories
{
    public class RentDocumentRepository : RepositoryBase<RentDocument>, IRentDocumentRepository
    {
        public RentDocumentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddDocument(RentDocument document) =>
            Create(document);

        public RentDocument FindDocumentByCondition(Expression<Func<RentDocument, bool>> expression, bool trackChanges) =>
            FindByCondition(expression, trackChanges)
            .FirstOrDefault();
    }
}
