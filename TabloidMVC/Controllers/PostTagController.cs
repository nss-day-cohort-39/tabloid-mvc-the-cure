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
                TagsOnPost = allTags,
                Post = post,
            };
            return View(vm);
        }

        // POST: PostTagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TagIndexViewModel vm, int id)
        {
            vm.Post = _postRepo.GetPublishedPostById(id);
            vm.TagsOnPost = _tagRepository.GetTagsByPostId(id);
            try
            {
                foreach(Tag t in vm.TagsOnPost)
                {
                    var postTag = _postTagRepo.GetPostTagByPostandTag(t.Id, id);
                    if(postTag != null)
                    {
                        _postTagRepo.DeletePostTag(postTag.Id);
                    }
                }
                foreach(int tagId in vm.AreChecked)
                {
                    var newPostTag = new PostTag()
                    {
                        PostId = id,
                        TagId = tagId
                    };
                }
                //foreach(Tag t in vm.TagsOnPost)
                //{
                //    if(t.Selected == true)
                //    {
                //        var postTag = _postTagRepo.GetPostTagByPostandTag(t.Id, id);
                //        if(postTag == null)
                //        {
                //            var newPostTag = new PostTag()
                //            {
                //                PostId = id,
                //                TagId = t.Id
                //            };
                //            _postTagRepo.AddPostTag(postTag);
                //        }
                //    }
                //    else
                //    {
                //        var postTag = _postTagRepo.GetPostTagByPostandTag(t.Id, id);
                //        if(postTag != null)
                //        {
                //            _postTagRepo.DeletePostTag(postTag.Id);
                //        }
                //    }
                //}
                return RedirectToAction("Details", "Post", new { id = id });
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
