using JobSeeking.Models;
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

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // View all users
        public IActionResult Index()
        {
            List<ApplicationUser> myList = _unitOfWork.ApplicationUserRepository.GetAll().ToList();
            return View(myList);
        }
        public IActionResult Details(string? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            ApplicationUser? user = _unitOfWork.ApplicationUserRepository.Get(c=>c.Id == id);
            if (user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        // Delete Account
        public async Task<IActionResult> Delete(string id)
        {
            try
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
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirement
                return StatusCode(500, "Internal server error");
            }
        }

        // Lock Account
        public async Task<IActionResult> LockAccount(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    user.isValid = !user.isValid;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirement
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
