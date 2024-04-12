using JobSeeking.Data;
using JobSeeking.Models;
using JobSeeking.Repository.IRepository;

namespace JobSeeking.Repository
{
    public class PostCategoryRepository:Repository<PostCategory>, IPostCategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public PostCategoryRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(PostCategory postCategory)
        {
            _db.PostCategories.Add(postCategory);
        }
    }
}
