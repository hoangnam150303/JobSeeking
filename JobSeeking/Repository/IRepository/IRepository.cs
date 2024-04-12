using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        IEnumerable<T> GetAll(string? includedProperty = null);
        T Get(Expression<Func<T, bool>> predicate, string? includedProperty = null);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        void Save();
    }
}
