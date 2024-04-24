using JobSeeking.Models;
using System.Linq.Expressions;

namespace JobSeeking.Repository.IRepository
{
    public interface IApplyCVRepository:IRepository<ApplyCV>
    {
        IEnumerable<ApplyCV> GetAllCV(Expression<Func<ApplyCV, bool>> filter = null);

        void Update(ApplyCV entity);
    }
   

}
