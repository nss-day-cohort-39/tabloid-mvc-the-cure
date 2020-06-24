using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class PostTagController : Controller
    {
        private readonly TagRepository _tagRepository;
        private readonly PostRepository _postRepo;


        public PostTagController(IConfiguration config)
        {
            _tagRepository = new TagRepository(config);
            _postRepo = new PostRepository(config);
        }

        // GET: PostTagController/Create
        public ActionResult Create(int id)
        {
            var tags = _tagRepository.GetTagsByPostId(id);
            var post = _postRepo.GetPublishedPostById(id);
            var vm = new TagIndexViewModel()
            {
                PostTags = tags,
                Post = post,
            };
            return View(vm);
        }

        // POST: PostTagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TagIndexViewModel vm, int id)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PostTagController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostTagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
