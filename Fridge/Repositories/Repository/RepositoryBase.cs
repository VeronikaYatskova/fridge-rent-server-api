using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace Fridge.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<T>? FindAll(bool trackChanges) =>
            !trackChanges ? RepositoryContext.Set<T>().AsNoTracking()
                          : RepositoryContext.Set<T>();

        public IQueryable<T>? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? RepositoryContext.Set<T>().Where(expression).AsNoTracking()
                          : RepositoryContext.Set<T>().Where(expression);

        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);

        public void ExecuteStoredProcedure(Guid productId, Guid userId)
        {
            var userGuidParam = new SqlParameter("@userId", userId);
            var productGuidParam = new SqlParameter("@productId", productId);

            var result = RepositoryContext.Database.ExecuteSqlRaw("EXEC AddProduct @userId, @productId", userGuidParam, productGuidParam);

            Console.WriteLine(result);
        }
    }
}
