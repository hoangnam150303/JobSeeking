using JobSeeking.Models;

namespace JobSeeking.Repository.IRepository
{
    public interface IPostCategoryRepository:IRepository<PostCategory>
    {
        void Update(PostCategory entity);
    }
}
