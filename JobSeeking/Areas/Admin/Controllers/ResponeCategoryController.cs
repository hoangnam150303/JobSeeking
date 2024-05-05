using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ResponeCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResponeCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Index category
        public IActionResult Index()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll("User").ToList();
            return View(categories);
        }
      //Method accept category
        public IActionResult Accept( int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                 category.isValid = true;
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.CategoryRepository.Save();
                TempData["success"] = " Accept category successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        //Method httpGet edit
        public IActionResult Edit(int? id)
        {
            if (id==null || id == 0)
            {
                return NotFound();
            }
            Category category = _unitOfWork.CategoryRepository.Get(c=>c.Id == id); 
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //Method httppost edit
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.CategoryRepository.Save();
                TempData["success"] = " Update category successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        //Delete Category
        public IActionResult Delete(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound() ; 
            }
            Category? category = _unitOfWork.CategoryRepository.Get(c=>c.Id==id);   
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.CategoryRepository.Delete(category);
                _unitOfWork.CategoryRepository.Save();
                TempData["success"] = " Delete category successfully!";
                return RedirectToAction("Index");
            }
            
        }
    }
}
