using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class SubController : Controller
    {
        private readonly SubRepository _subRepository;
        private readonly PostRepository _postRepository;

        public SubController(IConfiguration config)
        {
            _postRepository = new PostRepository(config);
            _subRepository = new SubRepository(config);
        }

        // GET: SubController
        public ActionResult Index()
        {
            //list of posts where Subscription.SubscriberUserProfileId = current user
            var cuId = GetCurrentUserProfileId();
            var subbedPosts = _postRepository.GetSubbedPosts(cuId);
            return View(subbedPosts);
        }

        // GET: SubController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SubController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SubController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
