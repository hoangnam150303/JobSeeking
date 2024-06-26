﻿using JobSeeking.Data;
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
        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }

        
    }
}
