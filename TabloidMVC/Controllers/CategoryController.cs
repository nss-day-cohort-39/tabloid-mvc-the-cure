using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly PostRepository _postRepository;

        public CategoryController(IConfiguration config)
        {
            _categoryRepository = new CategoryRepository(config);
            _postRepository = new PostRepository(config);
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAllCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category cgy)
        {
            try
            {
                _categoryRepository.Add(cgy);

                return RedirectToAction("Index", new { id = cgy.Id });
            }
            catch
            {
                return View(cgy);
            }
        }

    }
}
