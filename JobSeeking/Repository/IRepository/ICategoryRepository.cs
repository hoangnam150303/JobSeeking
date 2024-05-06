using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
         
        void Update(Category entity);
    }
   

}
