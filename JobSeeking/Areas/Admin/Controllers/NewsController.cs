using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<News> list = _unitOfWork.NewsRepository.GetAll().ToList();
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(IFormFile? file, News news)
        {
            return View();
        }
    }
}
