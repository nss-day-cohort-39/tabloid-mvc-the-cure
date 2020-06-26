using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {
        // GET: UserProfileController

        private readonly UserProfileRepository _UserProfileRepository;

        public UserProfileController(IConfiguration config)
        {
            _UserProfileRepository = new UserProfileRepository(config);
        }
        public ActionResult Index()
        {
            var userProfiles = _UserProfileRepository.GetAllUsers();
            return View(userProfiles);
        }

        // GET: UserProfileController/Details/5
        public ActionResult Details(int id)
        {
            var userProfile = _UserProfileRepository.GetUserById(id);
            if (userProfile.ImageLocation == null)
            {
                userProfile.ImageLocation = "https://static1.squarespace.com/static/54b7b93ce4b0a3e130d5d232/54b7cd91e4b0b6572f771175/5a9924d20d92970572b7c3b6/1519986489468/icon.png?format=500w";
            }
            return View(userProfile);

        }

        // GET: UserProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfileController/Create
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

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProfileController/Edit/5
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

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserProfileController/Delete/5
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

        public IActionResult DeactivateUser(int id)
        {
            //userId = userProfile.Id;
            var userProfile = _UserProfileRepository.GetUserById(id);
            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeactivateUser(int userId, UserProfile userProfile)
        {

            userId = userProfile.Id;
            userProfile= _UserProfileRepository.GetUserById(userId);
            List<UserProfile> userProfiles = _UserProfileRepository.GetAllUsers();

            int totalAdmins = userProfiles.Where(up => up.UserTypeId == 2).Count();
            try
            {

                if (userProfile.Activated == true)
                {
                    if (userProfile.UserTypeId == 2 && totalAdmins < 2)
                    {
                        ModelState.AddModelError("UserTypeId","Assign another admin before deactivating the final admin");
                        return View(userProfile);
                    }
                    else
                    {
                        _UserProfileRepository.DeactivateUser(userId);
                    }
                }
                else
                {
                    _UserProfileRepository.ReactivateUser(userId);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                // If something goes wrong, just keep the user on the same page so they can try again
                return View(userProfile);
            }

        }
    }
}
