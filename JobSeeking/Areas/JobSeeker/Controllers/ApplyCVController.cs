using JobSeeking.Migrations;
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
            List<Job> jobs = _unitOfWork.JobRepository.GetAll().ToList();   
            return View(jobs);
        }

        public IActionResult Create(int? id)
        {
            
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var job = _unitOfWork.JobRepository.Get(c => c.Id == id);
            var applyCv = _unitOfWork.ApplyCVRepository.Get(a=> a.JobId == id);
          
            if (job == null)
            {
                return NotFound();
            }
            if (applyCv != null && applyCv.JobId == job.Id)
            {
				TempData["error"] = "You have applied to this job!";
				return RedirectToAction("Index");
            }

            job.amountOfCV += 1;
            _unitOfWork.JobRepository.Update(job);
            _unitOfWork.JobRepository.Save();

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
                        // Check file extension
                        if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            ViewData["PdfValidationMessage"] = "Please upload a PDF file.";
                            return View(jobSeekingVM.applyCV);
                        }
                        // Check file size (max 5 MB)
                        if (file.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("File", "File size cannot exceed 5 MB.");
                            return View(jobSeekingVM.applyCV);
                        }
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string cvPath = Path.Combine(wwwrootPath, @"CV\CVsOfJob");

                        // Save the file
                        using (var fileStream = new FileStream(Path.Combine(cvPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        jobSeekingVM.applyCV.CV = @"\CV\CVsOfJob\" + fileName;
                    }
                        
                        _unitOfWork.ApplyCVRepository.Add(jobSeekingVM.applyCV);
                        _unitOfWork.ApplyCVRepository.Save();
                    TempData["success"] = " Apply CV successfully!";
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
                TempData["success"] = " Delete CV successfully!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id==null||id==0)
            {
                return NotFound();
            }
            ApplyCV applycv = _unitOfWork.ApplyCVRepository.Get(a=>a.Id == id);
            if (applycv == null)
            {
                return NotFound();
            }
            return View(applycv);
        }
        [HttpPost]
        public IActionResult Edit(ApplyCV editCV, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
               
                if (file != null && file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string cvPath = Path.Combine(wwwrootPath, @"CV\CVsOfJob");
                    if (!string.IsNullOrEmpty(editCV.CV))
                    {
                        var oldCVPath = Path.Combine(wwwrootPath, editCV.CV.TrimStart('\\'));
                        if (System.IO.File.Exists(oldCVPath))
                        {
                            System.IO.File.Delete(oldCVPath);
                        }
                        // Check file extension
                        if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            ViewData["PdfValidationMessage"] = "Please upload a PDF file.";
                            return View(editCV);
                        }
                        // Check file size (max 5 MB)
                        if (file.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("File", "File size cannot exceed 5 MB.");
                            return View(editCV);
                        }
                        // Save the file
                        using (var fileStream = new FileStream(Path.Combine(cvPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        editCV.CV = @"\CV\CVsOfJob\" + fileName;
                    }

                   
                }
                _unitOfWork.ApplyCVRepository.Update(editCV);
                _unitOfWork.ApplyCVRepository.Save();
                TempData["success"] = " Update CV successfully!";
                return RedirectToAction("Index");
            }
            return View(editCV);
        }
        public IActionResult DownloadCV(string cvPath)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, cvPath.TrimStart('\\', '/'));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileContent = System.IO.File.ReadAllBytes(filePath);
            return File(fileContent, "application/pdf", Path.GetFileName(filePath));
        }
        public IActionResult FindJob(string nameJob)
        {
            if (string.IsNullOrEmpty(nameJob))
            {
                return NotFound();
            }
            var job = _unitOfWork.JobRepository.GetAll().Where(j=>j.Name == nameJob).ToList();
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }
    }

}
