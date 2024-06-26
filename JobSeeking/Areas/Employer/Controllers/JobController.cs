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
            List<Job> jobs = _unitOfWork.JobRepository.GetAll().ToList();
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
                    TempData["success"] = " Create job successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
					TempData["error"] = " Create job unsuccessfully!";
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
            return View(jobSeekingVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JobSeekingVM jobSeeking)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.JobRepository.Update(jobSeeking.Job);
                _unitOfWork.JobRepository.Save();
                TempData["success"] = " Update news successfully!";
                return RedirectToAction("Index");
            }
            else
            {
				TempData["error"] = " Edit category fail!";
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
				TempData["error"] = " Delete category unsuccessfully!";
				return RedirectToAction("Index");
            }
            else
            {
                _unitOfWork.JobRepository.Delete(job);
                _unitOfWork.JobRepository.Save();
                TempData["success"] = " Delete news successfully!";
                return RedirectToAction("Index");
            }
        }
        public IActionResult ViewAllCV(int? id, bool? status)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Job job = _unitOfWork.JobRepository.Get(j => j.Id == id);
            job.amountOfCV = 0;
            _unitOfWork.JobRepository.Update(job);
            _unitOfWork.JobRepository.Save();
            List<ApplyCV> mylist = _unitOfWork.ApplyCVRepository.GetAll().ToList();
            List<ApplyCV> applyCVs = new List<ApplyCV>();
            if (status.HasValue) 
            {
                if (status == true)
                {
                    applyCVs = mylist.Where(cv => cv.JobId == job.Id && cv.Status == true).ToList();
                }
                else
                {
                    applyCVs = mylist.Where(cv => cv.JobId == job.Id && cv.Status == false).ToList();
                }
            }
            else 
            {
                applyCVs = mylist.Where(cv => cv.JobId == job.Id && cv.Status == null).ToList();
            }

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
            TempData["success"] = " Accept CV successfully!";
            return RedirectToAction("Index");
        }
        public IActionResult DeclineCV(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplyCV? applyCV = _unitOfWork.ApplyCVRepository.Get(c => c.Id == id);
            if (applyCV == null)
            {
                return NotFound();
            }
            applyCV.Status = false;
            _unitOfWork.ApplyCVRepository.Update(applyCV);
            _unitOfWork.ApplyCVRepository.Save();
            TempData["success"] = " Decline CV successfully!";
            return RedirectToAction("Index");
        }

        public async Task< IActionResult> DetailOfJobSeeker(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var emailUser = _unitOfWork.ApplyCVRepository.Get(c => c.Id == id);
            var detailUser = await _userManager.FindByEmailAsync(emailUser.JobSeekerEmail);
            if (detailUser==null)
            {
                return NotFound();
            }
            return View(detailUser);    
        }
    }
}

