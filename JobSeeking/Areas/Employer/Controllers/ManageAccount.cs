using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer,JobSeeker")]
    public class ManageAccount : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public ManageAccount(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
        public async Task<IActionResult> Update(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult Update(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
               _unitOfWork.ApplicationUserRepository.Update(user);
                _unitOfWork.ApplicationUserRepository.Save();
                return RedirectToAction("Index", "Home", new { area = "JobSeeker" });
            }
            return View(user);
        }

    }
}
