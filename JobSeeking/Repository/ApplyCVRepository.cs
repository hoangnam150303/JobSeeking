using JobSeeking.Data;
using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace JobSeeking.Repository
{
    public class ApplyCVRepository:Repository<ApplyCV>,IApplyCVRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplyCVRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(ApplyCV entity)
        {
            _db.ApplyCV.Update(entity);
        }
    }
}
