using JobSeeking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobSeeking.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
      
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ApplyCV> ApplyCV { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id=1,Name="IT",CreateDay=new DateTime()}
                );
            modelBuilder.Entity<Job>().HasData(
                new Job { Id=1,Name="Software Engineer",Description="Java,C#,JS",Salary=20.000,CompanyName="FPT Software",Logo=null,CategoryId=1},
                new Job { Id=2,Name="Machine Learning Engineer",Description="Python",Salary=20.000,CompanyName="FPT Software",Logo=null,CategoryId=1}
                );
        }
    }
}
