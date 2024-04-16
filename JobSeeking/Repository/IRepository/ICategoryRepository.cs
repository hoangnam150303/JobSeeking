using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        IEnumerable<Category> GetAllWithUser(Expression<Func<Category, bool>> predicate = null);

         string GetCategoryNameById(int categoryId);
        void Update(Category entity);
    }
   

}
