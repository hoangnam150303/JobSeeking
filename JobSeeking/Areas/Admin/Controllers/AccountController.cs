

using JobSeeking.Models;
using JobSeeking.Models.ViewModels;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
      
        private readonly IUnitOfWork _unitOfWork;
  
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) {
            _unitOfWork = unitOfWork;
 
            _userManager = userManager;           
        }
        //View all user 
        public IActionResult Index()
        {
            List<ApplicationUser> myList = _unitOfWork.ApplicationUserRepository.GetAll().ToList();
            return View(myList);
        }
        //Delete Account
       public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("Index");
            }
        }
        //Lock Account
        public async Task<IActionResult> LockAccount(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
           else
            {
                user.isValid = false;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
        }

    }
}
