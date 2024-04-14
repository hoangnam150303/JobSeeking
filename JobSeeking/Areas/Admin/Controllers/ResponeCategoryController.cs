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
        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(categories);
        }
        [HttpPost]
        public IActionResult Accept(int id)
        {   
            var category = _unitOfWork.CategoryRepository.Get(c=>c.Id==id);
            if (category == null)
            {
                return NotFound();
            }
            category.isValid = true;
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.CategoryRepository.Save();
            return RedirectToAction("ResponeCategory","Index");
        }
    }
}
