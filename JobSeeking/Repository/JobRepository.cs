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
        public void Update(Job job)
        {
            _db.Jobs.Update(job);
        }
    }
}
