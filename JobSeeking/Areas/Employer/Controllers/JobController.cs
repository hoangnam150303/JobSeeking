using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Employer.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
