using JobSeeking.Models;
using JobSeeking.Models.ViewModels;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobSeeking.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class JobController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var jobs = _unitOfWork.JobRepository.GetAll().Where(j => j.EmployerId == currentUser.Id).ToList();
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
            JobSeekingVM jobSeekingVM = new JobSeekingVM
            {
                Categories = _unitOfWork.CategoryRepository.GetAll().Where(c => c.isValid).Select(c => new SelectListItem() 
               {
                    Text = c.Name, 
                    Value = c.Id.ToString(),
                })
            };

            return View(jobSeekingVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobSeekingVM jobSeeking)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    jobSeeking.Job.EmployerId = currentUser.Id;
                    jobSeeking.Job.Logo = currentUser.Avatar;
                    jobSeeking.Job.CompanyName = currentUser.Company;
                    _unitOfWork.JobRepository.Add(jobSeeking.Job);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            return View(jobSeeking);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobSeekingVM jobSeekingVM = new JobSeekingVM()
            {
                Categories = _unitOfWork.CategoryRepository.GetAll().Where(c => c.isValid).Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }),
                Job = _unitOfWork.JobRepository.Get(c => c.Id == id)
            };
            jobSeekingVM.Categories = _unitOfWork.CategoryRepository.GetAll().Where(c => c.isValid).Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
            return View(jobSeekingVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(JobSeekingVM jobSeeking)
        {
           
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                jobSeeking.Job.CompanyName = currentUser.Company;
                jobSeeking.Job.EmployerId = currentUser.Id;
                _unitOfWork.JobRepository.Update(jobSeeking.Job);
                _unitOfWork.JobRepository.Save();
                return RedirectToAction("Index");
            }
            return View(jobSeeking);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Job? job = _unitOfWork.JobRepository.Get(c=>c.Id == id);
            if (job==null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.JobRepository.Delete(job);
                _unitOfWork.JobRepository.Save();
                return RedirectToAction("Index");
            }
            
        }
    }
}

