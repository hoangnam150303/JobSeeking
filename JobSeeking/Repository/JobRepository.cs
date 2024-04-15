using JobSeeking.Data;
using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace JobSeeking.Repository
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        private readonly ApplicationDbContext _db;
        public JobRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<Job> GetAllWithUser(Expression<Func<Job, bool>> predicate = null)
        {
            IQueryable<Job> query = _db.Jobs.Include(c => c.User);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query.ToList();
        }

        public void Update(Job job)
        {
            _db.Jobs.Update(job);
        }
    }
}
