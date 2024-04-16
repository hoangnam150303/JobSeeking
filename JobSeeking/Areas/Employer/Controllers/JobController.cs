using JobSeeking.Models;
using JobSeeking.Models.ViewModels;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class JobController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager <ApplicationUser> _userManager;
        public JobController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<Job> jobs = _unitOfWork.JobRepository.GetAll().ToList();
            foreach (var job in jobs)
            {
                var categories = new List<string>();

                foreach (var categoryIdString in job.Category)
                {
                   int categoryId = int.Parse(categoryIdString);
                    var categoryName = _unitOfWork.CategoryRepository.GetCategoryNameById(categoryId);
                    if (categoryName != null)
                    {
                        categories.Add(categoryName);
                    }
                }
                job.Category = categories.ToArray();
            }

            return View(jobs);
        }
        public IActionResult Create()
        {
            JobSeekingVM jobSeekingVM = new JobSeekingVM() {
                Category = _unitOfWork.CategoryRepository.GetAll().Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }),
                Job = new Job()

            };
            return View(jobSeekingVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Job job)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    job.EmployerId = currentUser.Id;
                    _unitOfWork.JobRepository.Add(job);
                    _unitOfWork.JobRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            
            return View(job);
        }
    }
}

