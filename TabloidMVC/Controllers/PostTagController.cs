using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class PostTagController : Controller
    {
        private readonly TagRepository _tagRepository;
        private readonly PostRepository _postRepo;
        private readonly PostTagRepository _postTagRepo;


        public PostTagController(IConfiguration config)
        {
            _tagRepository = new TagRepository(config);
            _postRepo = new PostRepository(config);
            _postTagRepo = new PostTagRepository(config);
        }

        // GET: PostTagController/Create
        public ActionResult Create(int id)
        {
            var allTags = _tagRepository.GetAllTags();
            //get all relationships belonging to this post (PostTag)
            var postTags = _postTagRepo.GetPostTagsByPostId(id);
            List<int> foundTagIds = new List<int>();

            //1. take the TagId's from each PostTag and add to list of id ints = has rel to this Post
            foreach(PostTag pt in postTags)
            {
                foundTagIds.Add(pt.TagId);
            }
            //2. take the tagId's that are not associated and add to different list of int Ids
            // loop through allTags, if Id is present in list 1 assign boolean true, if not assign boolean false (contains)
            foreach(Tag t in allTags)
            {
                var tId = t.Id;
                if(foundTagIds.Contains(tId))
                {
                    t.Selected = true;
                }
                else
                {
                    t.Selected = false;
                }
            }

            var post = _postRepo.GetPublishedPostById(id);
            var vm = new TagIndexViewModel()
            {
                PostTags = allTags,
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
