﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Repositories;
using System;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Models;

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

        public IActionResult Edit(int id, int postId)
        {
            CommentIndexViewModel commVM = new CommentIndexViewModel();
            commVM.Comment = _commentRepo.GetCommentById(id);
            commVM.Comment.PostId = postId;
            return View(commVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CommentIndexViewModel commVM, int id)
        {
            try
            {
                commVM.Comment.Id = id;
                commVM.Comment.PostId = _commentRepo.GetCommentById(id).PostId;
                commVM.Comment.UserProfileId = GetCurrentUserProfileId();
                commVM.Comment.CreateDateTime = DateTime.Now;
                _commentRepo.EditComment(commVM.Comment);
                return RedirectToAction("Index", new { id = commVM.Comment.PostId });
            }
            catch
            {
                return View(commVM);
            }
        }

        public IActionResult Delete(int id)
        {
            var comment = _commentRepo.GetCommentById(id);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Comment comment, int id)
        {
            try
            {
                comment.PostId = _commentRepo.GetCommentById(id).PostId;
                _commentRepo.DeleteComment(id);
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
