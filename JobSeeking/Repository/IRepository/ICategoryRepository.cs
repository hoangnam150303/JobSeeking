using JobSeeking.Models;

namespace JobSeeking.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category entity);
    }
}
