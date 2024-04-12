using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Employer.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
