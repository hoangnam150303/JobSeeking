using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.JobSeeker.Controllers
{
    public class ApplyCVController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
