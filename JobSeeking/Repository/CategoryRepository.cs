using JobSeeking.Data;
using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace JobSeeking.Repository
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext  _db;
        public CategoryRepository(ApplicationDbContext db):base(db) 
        { 
            _db = db;
        }

        public IEnumerable<Category> GetAllWithUser(Expression<Func<Category, bool>> predicate = null)
        {
            IQueryable<Category> query = _db.Categories.Include(c => c.User);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query.ToList();
        }
        public string GetCategoryNameById(int categoryId)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
            return category?.Name;
        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }

        
    }
}
