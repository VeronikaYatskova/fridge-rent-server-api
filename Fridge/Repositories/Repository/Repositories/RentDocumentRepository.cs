using Fridge.Models;
using Fridge.Repository.Interfaces;
using System.Linq.Expressions;

namespace Fridge.Repository.Repositories
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
