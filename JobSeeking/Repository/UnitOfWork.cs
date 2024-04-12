﻿using JobSeeking.Data;
using JobSeeking.Repository.IRepository;

namespace JobSeeking.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _db;
        public ICategoryRepository CategoryRepository { get; private set; }
        public IJobRepository JobRepository { get; private set; }
        public IPostCategoryRepository PostCategoryRepository { get; private set; }
        public INewsRepository PostsRepository { get; private set; }
        public IApplicationUserRepository UserRepository { get; private set; }

        public INewsRepository NewsRepository {  get; private set; }

        public IApplicationUserRepository ApplicationUserRepository {  get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(db);
            JobRepository = new JobRepository(db);
            PostCategoryRepository = new PostCategoryRepository(db);
            ApplicationUserRepository = new ApplicationUserRepository(db);
            NewsRepository = new NewsRepository(db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
