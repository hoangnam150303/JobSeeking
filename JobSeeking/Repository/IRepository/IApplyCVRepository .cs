using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface IApplyCVRepository:IRepository<ApplyCV>
    {
        void Update(ApplyCV entity);
    }
   

}
