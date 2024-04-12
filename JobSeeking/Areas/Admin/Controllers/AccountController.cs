

using JobSeeking.Models;
using JobSeeking.Models.ViewModels;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
      
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;    

        public AccountController(IUnitOfWork unitOfWork,RoleManager<IdentityRole> roleManager) {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
           
        }

        public IActionResult Index()
        {
            List<ApplicationUser> myList = _unitOfWork.ApplicationUserRepository.GetAll().ToList();
            return View(myList);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null||id==0)
            {
                return NotFound();
            }

            return View(id);
        }

    }
}
