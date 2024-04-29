using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
         string GetCategoryNameById(int categoryId);
        void Update(Category entity);
    }
   

}
