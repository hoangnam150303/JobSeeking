using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    [Authorize(Roles = "Job Seeker")]
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
            return View();
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = _unitOfWork.JobRepository.Get(c => c.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            var applyCV = new ApplyCV
            {
                JobId = job.Id
            };

            return View(applyCV);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplyCV applyCV, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    applyCV.JobSeekerEmail = currentUser.Email;
                    if (file != null && file.Length > 0)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string cvPath = Path.Combine(wwwrootPath, @"CV\ApplyCV");
                        using (var fileStream = new FileStream(Path.Combine(cvPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        applyCV.CV = @"\CV\ApplyCV\" + fileName;
                    }

                    _unitOfWork.ApplyCVRepository.Add(applyCV);
                    _unitOfWork.ApplyCVRepository.Save();

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(applyCV);
        }
    }

}
