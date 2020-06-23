using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using System;
using System.Collections.Generic;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepo;
        private int postId;

        public CommentController(IConfiguration config)
        {
            _commentRepo = new CommentRepository(config);
        }

        public IActionResult Index()
        {
            var comments = _commentRepo.GetCommentsByPostId(postId);
            return View(comments);
        }

        public IActionResult Create()
        {
            var comment = new Comment();
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                _commentRepo.AddComment(comment);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }
    }
}
