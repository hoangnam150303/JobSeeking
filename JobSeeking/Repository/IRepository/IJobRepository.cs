using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface IJobRepository:IRepository<Job>
    {
        IEnumerable<Job> GetAllWithUser(Expression<Func<Job, bool>> predicate = null);
        void Update(Job entity);    
    }
}
