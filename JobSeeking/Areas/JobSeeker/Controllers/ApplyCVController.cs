using JobSeeking.Models;
using JobSeeking.Models.ViewModels;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobSeeking.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    [Authorize(Roles = "JobSeeker")]
    public class ApplyCVController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ApplyCVController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var jobs = _unitOfWork.JobRepository.GetAll().ToList();
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

        public IActionResult Create(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var job = _unitOfWork.JobRepository.Get(c => c.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            var jobSeekingVM = new JobSeekingVM();
            jobSeekingVM.applyCV = new ApplyCV();
            jobSeekingVM.applyCV.JobId = job.Id;
            return View(jobSeekingVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobSeekingVM jobSeekingVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    jobSeekingVM.applyCV.JobSeekerEmail = currentUser.Email;

                    if (file != null && file.Length > 0)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string cvPath = Path.Combine(wwwrootPath, @"images\CV");

                        using (var fileStream = new FileStream(Path.Combine(cvPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        jobSeekingVM.applyCV.CV = @"\images\CV\" + fileName;
                    }
                    _unitOfWork.ApplyCVRepository.Add(jobSeekingVM.applyCV);
                    _unitOfWork.ApplyCVRepository.Save();
                    return RedirectToAction("Index");
                }
            }

            return View(jobSeekingVM.applyCV);
        }

        public async Task<IActionResult> ListCV()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            List<ApplyCV> myList = _unitOfWork.ApplyCVRepository.GetAll("Job").Where(c => c.JobSeekerEmail == currentUser.Email).ToList();
            return View(myList);
        }
        public IActionResult ViewDetailOfJob(int? id)
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

        public IActionResult Delete(int? id)
        {
            if (id == null||id==0)
            {
                return NotFound();
            }
            ApplyCV? applyCV = _unitOfWork.ApplyCVRepository.Get(c=>c.Id == id);
            if (applyCV == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.ApplyCVRepository.Delete(applyCV);
                _unitOfWork.ApplyCVRepository.Save();
                return RedirectToAction("Index");
            }
        }
    }

}
