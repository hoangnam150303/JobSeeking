﻿using JobSeeking.Models;
using JobSeeking.Models.ViewModels;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace JobSeeking.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class JobController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JobController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Edit(JobSeekingVM jobSeeking)
        {

            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
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
            Job? job = _unitOfWork.JobRepository.Get(c => c.Id == id);
            if (job == null)
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
        public IActionResult ViewAllCV(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Expression<Func<ApplyCV, bool>> filter = c => c.JobId == id;
            var applyCVs = _unitOfWork.ApplyCVRepository.GetAllCV(filter);
            return View(applyCVs);
        }
        public IActionResult Detail(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplyCV? cv = _unitOfWork.ApplyCVRepository.Get(c=>c.Id == id);
            if (cv==null)
            {
                return NotFound(); 
            }
            return View(cv);
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
        public IActionResult checkAmountOfCV(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var job = _unitOfWork.JobRepository.Get(c=>c.Id==id);
            if (job==null)
            {
                return NotFound();
            }
            job.amountOfCV = 0;
            return View("ViewAllCV");
        }
        public IActionResult AcceptCV(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplyCV? applyCV = _unitOfWork.ApplyCVRepository.Get(c=>c.Id==id);
            if (applyCV == null)
            {
                return NotFound();
            }
            applyCV.Status = true;
            _unitOfWork.ApplyCVRepository.Update(applyCV);
            _unitOfWork.ApplyCVRepository.Save();
            return RedirectToAction("Index");
        }
    }
}

