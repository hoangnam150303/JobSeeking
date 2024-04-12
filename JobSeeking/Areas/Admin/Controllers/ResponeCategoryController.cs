using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Admin.Controllers
{
    public class ResponeCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
