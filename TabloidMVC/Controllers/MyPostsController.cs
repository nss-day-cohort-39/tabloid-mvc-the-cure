using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class MyPostsController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly CategoryRepository _categoryRepository;

        public MyPostsController(IConfiguration config)
        {
            _postRepository = new PostRepository(config);
            _categoryRepository = new CategoryRepository(config);
        }

        public IActionResult Index()
        {
            int UserId = GetCurrentUserProfileId();
            var posts = _postRepository.GetCurrentUsersPosts(UserId);
            
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAllCategories();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAllCategories();
                return View(vm);
            }
        }
        public IActionResult Delete(int id)
        {
            //int UserProfileId = GetCurrentUserProfileId();
            var post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction("Index");
            }
            catch
            {
                // If something goes wrong, just keep the user on the same page so they can try again
                return View(post);
            }
        }

        public IActionResult Edit(int id)
        {
            PostCreateViewModel vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAllCategories();
            vm.Post = _postRepository.GetPublishedPostById(id);

            return View(vm);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                vm.CategoryOptions = _categoryRepository.GetAllCategories();
                vm.Post.UserProfileId = GetCurrentUserProfileId();
                vm.Post.Id = id;



                vm.Post.CreateDateTime = DateAndTime.Now;

                _postRepository.UpdatePost(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAllCategories();
                return View(vm);
            }


        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
