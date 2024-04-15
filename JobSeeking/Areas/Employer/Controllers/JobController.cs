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
            List<Job> myList = _unitOfWork.JobRepository.GetAll().ToList();
            return View(myList);
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
        public async Task<IActionResult> CreateAsync(JobSeekingVM model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    model.Job.EmployerId = currentUser.Id;
                    _unitOfWork.JobRepository.Add(model.Job);
                    _unitOfWork.JobRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            model.Category = _unitOfWork.CategoryRepository.GetAll().Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            }
            );
            return View(model);
        }
    }
}

