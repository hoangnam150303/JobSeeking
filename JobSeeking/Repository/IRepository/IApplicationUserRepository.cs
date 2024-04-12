using JobSeeking.Models;

namespace JobSeeking.Repository.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        void Update(ApplicationUser entity);
    }
}
