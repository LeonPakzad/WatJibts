using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;
using Microsoft.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace src.Controllers {
    public class UserController : Controller
    {
        private readonly WatDbContext _context;

        public UserController(WatDbContext context)
        {
            _context = context;
        }

        public IActionResult UserIndex()
        {
            return View(_context.User.ToList());
        }

        public IActionResult Profile()
        {
            string userId = HttpContext.User.Identity.Name;

            if(userId == null) {
                return NotFound();
            }
            
            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);
            ViewBag.locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);

            UserProfile userProfile = new UserProfile();
            userProfile.User = _context.User.Find(userId);
            userProfile.userLunchSessions = _context.LunchSession.ToList().Where(l =>l.fk_user == userId);

            return View(userProfile);
        }

        [HttpPost]
        public IActionResult Profile(User user)
        {
            var currentUser = _context.User.Where(u => u.Id == user.Id).FirstOrDefault();
            
            currentUser.UserName = user.UserName;
            currentUser.fk_defaultPlaceToEat = user.fk_defaultPlaceToEat;
            currentUser.fk_defaultPlaceToGetFood = user.fk_defaultPlaceToGetFood;
            currentUser.preferredLunchTime = user.preferredLunchTime;

            _context.SaveChanges();
            
            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);
            ViewBag.locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);



            return View(user);
        }

        public ActionResult Edit(string Id)
        {
            var user = _context.User.Where(u => u.Id == Id).FirstOrDefault();

            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);
            ViewBag.locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            var currentUser = _context.User.Where(u => u.Id == user.Id).FirstOrDefault();
            
            currentUser.UserName = user.UserName;
            currentUser.fk_defaultPlaceToEat = user.fk_defaultPlaceToEat;
            currentUser.fk_defaultPlaceToGetFood = user.fk_defaultPlaceToGetFood;
            currentUser.preferredLunchTime = user.preferredLunchTime;

            _context.SaveChanges();

            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);
            ViewBag.locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);

            return RedirectToAction("UserIndex");
        }

        public ActionResult Delete(string id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            _context.SaveChanges();
            
            return RedirectToAction("UserIndex");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}