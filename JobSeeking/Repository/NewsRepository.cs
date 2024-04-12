using JobSeeking.Data;
using JobSeeking.Models;
using JobSeeking.Repository.IRepository;

namespace JobSeeking.Repository
{
    public class NewsRepository:Repository<News>,INewsRepository
    {
        private readonly ApplicationDbContext _db;
        public NewsRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(News news)
        {
            _db.News.Update(news);
        }
    }
}
