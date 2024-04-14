using JobSeeking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobSeeking.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
      
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ApplyCV> ApplyCV { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
       
    }
}
