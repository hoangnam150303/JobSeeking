using JobSeeking.Data;
using JobSeeking.Models;
using JobSeeking.Repository.IRepository;

namespace JobSeeking.Repository
{
    public class ApplicationUserRepository:Repository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(ApplicationUser user)
        {
            _db.applicationUsers.Update(user);
        }
    }
}
