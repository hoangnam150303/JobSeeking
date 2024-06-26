﻿using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace JobSeeking.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<Category> myList = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(myList);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null) {
                    category.EmployerId = currentUser.Id;
                    category.isValid = false;
                    _unitOfWork.CategoryRepository.Add(category);
                    _unitOfWork.Save();
                    TempData["success"] = " Create category successfully!";
                    return RedirectToAction("Index");
                }
                else
				{
					TempData["error"] = " Create category unsuccessfully!";
					return RedirectToAction("Index");
                }
            }
            return View(category);
        }
        public IActionResult ToggleNotification(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            category.categoryValid = !category.categoryValid;
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}
