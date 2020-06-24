using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Repositories;
using System;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepo;
        private readonly PostRepository _postRepo;

        public CommentController(IConfiguration config)
        {
            _commentRepo = new CommentRepository(config);
            _postRepo = new PostRepository(config);
        }

        public IActionResult Index(int id)
        {
            var comments = _commentRepo.GetCommentsByPostId(id);
            var post = _postRepo.GetPublishedPostById(id);
            var commVM = new CommentIndexViewModel()
            {
                PostComments = comments,
                Post = post,
            };
            return View(commVM);
        }

        public IActionResult Create(int id)
        {
            var commVM = new CommentIndexViewModel();
            commVM.PostComments = _commentRepo.GetCommentsByPostId(id);
            return View(commVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentIndexViewModel commVM, int id)
        {
            try
            {
                commVM.Comment.UserProfileId = GetCurrentUserProfileId();
                commVM.Comment.PostId = id;
                commVM.PostComments = _commentRepo.GetCommentsByPostId(id);
                commVM.Comment.CreateDateTime = DateTime.Now;
                _commentRepo.AddComment(commVM.Comment);
                return RedirectToAction("Index", new { id = id });
            }
            catch (Exception ex)
            {
                commVM.PostComments = _commentRepo.GetCommentsByPostId(id);
                return View(commVM);
            }
        }

        public IActionResult Edit(int id)
        {
            CommentIndexViewModel commVM = new CommentIndexViewModel();
            commVM.Comment = _commentRepo.GetCommentById(id);
            return View(commVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CommentIndexViewModel commVM, int id)
        {
            try
            {
                commVM.Comment.UserProfileId = GetCurrentUserProfileId();
                commVM.Comment.CreateDateTime = DateTime.Now;
                _commentRepo.EditComment(commVM.Comment);
                return RedirectToAction("Details", new { id = commVM.Comment.Id });
            }
            catch
            {
                return View(commVM);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
