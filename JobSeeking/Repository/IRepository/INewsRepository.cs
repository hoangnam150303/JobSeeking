using JobSeeking.Models;

namespace JobSeeking.Repository.IRepository
{
    public interface INewsRepository:IRepository<News>
    {
        void Update(News entity);
    }
}
