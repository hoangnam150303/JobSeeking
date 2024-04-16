using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface IJobRepository:IRepository<Job>
    {
        
        void Update(Job entity);    
    }
}
