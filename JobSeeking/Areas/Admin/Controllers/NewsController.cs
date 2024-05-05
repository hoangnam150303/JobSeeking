using JobSeeking.Models;
using JobSeeking.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(IFormFile? file, News news)
        {
            string wwwrootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string newsPath = Path.Combine(wwwrootPath, @"images\ImagesOfNews");
                using (var fileStream = new FileStream(Path.Combine(newsPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                news.image = @"\images\ImagesOfNews\" + fileName;
                _unitOfWork.NewsRepository.Add(news);
                _unitOfWork.NewsRepository.Save();
                TempData["success"] = " Create news successfully!";
                return RedirectToAction("Index");
            }
            return View(news);      
    }
        public IActionResult Edit(int? id)
        {
            if (id==null||id==0)
            {
                return NotFound();
            }
            News ? news = _unitOfWork.NewsRepository.Get(c => c.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
        [HttpPost]
        public IActionResult Edit(IFormFile? file, News news)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string newsPath = Path.Combine(wwwrootPath, @"images\ImagesOfNews");
                    //Delete Old Images
                    if (!string.IsNullOrEmpty(news.image))
                    {
                        var oldImagePath = Path.Combine(wwwrootPath, news.image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //Copy File to \img\Books
                    using (var fileStream = new FileStream(Path.Combine(newsPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //Update ImageUrl in DB
                    news.image = @"\images\ImagesOfNews\" + fileName;
                }
                _unitOfWork.NewsRepository.Update(news);
                _unitOfWork.NewsRepository.Save();
                TempData["success"] = " Update news successfully!";
                return RedirectToAction("Index");   
            }
            return View(news);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            News? news = _unitOfWork.NewsRepository.Get(c => c.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.NewsRepository.Delete(news);
                _unitOfWork.NewsRepository.Save();
                TempData["success"] = " Delete news successfully!";
                return RedirectToAction("Index");
            }

        }
    }
}
