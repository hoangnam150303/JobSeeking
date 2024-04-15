using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        IEnumerable<Category> GetAllWithUser(Expression<Func<Category, bool>> predicate = null);
        void Update(Category entity);
    }
   

}
