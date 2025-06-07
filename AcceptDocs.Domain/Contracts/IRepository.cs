using System.Linq.Expressions;

namespace AcceptDocs.Domain.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Count();
        TEntity Get(int id);
        IList<TEntity> GetAll();
        IList<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        TEntity Insert(TEntity entity);
        void Delete(TEntity entity);
    }
}
