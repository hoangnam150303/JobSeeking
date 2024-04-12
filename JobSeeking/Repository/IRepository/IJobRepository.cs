using JobSeeking.Models;

namespace JobSeeking.Repository.IRepository
{
    public interface IJobRepository:IRepository<Job>
    {
        void Update(Job entity);    
    }
}
