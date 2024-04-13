using JobSeeking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using JobSeeking.Repository.IRepository;
using JobSeeking.Repository;
using JobSeeking.Ultility;
using Microsoft.AspNetCore.Identity.UI.Services;
using JobSeeking.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorPages();

builder.Services.AddIdentity <ApplicationUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    option.LoginPath = $"/Identity/Account/Login";
    option.LogoutPath = $"/Identity/Account/Logout";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=JobSeeker}/{controller=Home}/{action=Index}/{id?}");
app.Run();
