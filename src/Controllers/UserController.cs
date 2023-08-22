using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;
using Microsoft.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace src.Controllers {
    public class UserController : Controller
    {
        private readonly WatDbContext _context;

        public UserController(WatDbContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserIndex()
        {
            return View(_context.User.ToList());
        }

        [Authorize]
        public IActionResult Profile()
        {
            UserProfile userProfile = new UserProfile();

            // add default Lunchsessions if they are not created yet
            if(!_context.LunchSession.Where(l => l.fk_user == getCurrentUserId() && l.isDefault == true).Any())
            {
                int daysOfWeek = 7;
                int defaultPlacefk = -1;

                for(int weekdayIndex = 1; weekdayIndex <= daysOfWeek; weekdayIndex++)
                {
                    LunchSession defaultLunchSession = new LunchSession
                    {
                        lunchTime = new DateTime(),
                        participating = false,
                        fk_foodPlace = -1,
                        fk_eatingPlace = -1,
                        fk_user = getCurrentUserId(),
                        isDefault = true,
                        weekday = weekdayIndex
                    };

                    _context.Add(defaultLunchSession);
                }
                _context.SaveChanges();
            }

            // check if admin exists to enable first admin
            ViewBag.adminExists = _context.UserRoles.Any(u => u.RoleId != null);

            // add locations for 
            ViewBag.locationsToEat      = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
            ViewBag.locationsToGetFood  = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

            userProfile.user = _context.User.Where(p => p.Id == getCurrentUserId()).First();
            userProfile.userLunchSessions = _context.LunchSession.ToList().Where(l =>l.fk_user == getCurrentUserId() & l.isDefault == false);

            userProfile.defaultLunchSessions = _context.LunchSession.ToList().Where(l =>l.fk_user == getCurrentUserId() & l.isDefault == true);

            return View(userProfile);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Profile(User user)
        {
            var currentUser = _context.User.Where(u => u.Id == getCurrentUserId()).First();
            
            currentUser.UserName = user.UserName;
            currentUser.fk_defaultPlaceToEat = user.fk_defaultPlaceToEat;
            currentUser.fk_defaultPlaceToGetFood = user.fk_defaultPlaceToGetFood;
            currentUser.preferredLunchTime = user.preferredLunchTime;
            _context.SaveChanges();
            
            return RedirectToAction("Profile");
        }

        [Authorize]
        public async Task<IActionResult> AddFirstAdmin(User user)
        {
            var userRole = new IdentityUserRole<string>();
            userRole.UserId = _context.User.Where(u => u.Id == getCurrentUserId()).Select(u => u.Id).FirstOrDefault();
            userRole.RoleId = _context.Roles.Where(r => r.Name == "Admin").Select(r => r.Id).FirstOrDefault();
            _context.UserRoles.Add(userRole);
            
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        [Authorize]
        public async Task<IActionResult> PromoteToAdmin(string Id)
        {
            var userRole = new IdentityUserRole<string>();
            userRole.UserId = Id;
            userRole.RoleId = _context.Roles.Where(r => r.Name == "Admin").Select(r => r.Id).FirstOrDefault();
           
            _context.UserRoles.Add(userRole);
            
            _context.SaveChanges();

            return RedirectToAction("UserIndex");
        }

        [Authorize]
        public async Task<IActionResult> DemoteToUser(string Id)
        {
            _context.UserRoles.Remove( _context.UserRoles.Where(r => r.UserId == Id).First());
            
            _context.SaveChanges();

            return RedirectToAction("UserIndex");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string Id)
        {
            
            var user = _context.User.Where(u => u.Id == Id).FirstOrDefault();

            if(user.Id == getCurrentUserId())
            {
                return RedirectToAction("Profile");
            }

            ViewBag.isAdmin = _context.UserRoles.Where(u => u.UserId == user.Id).Any();
            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);
            ViewBag.locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(User user)
        {
            var currentUser = _context.User.Where(u => u.Id == getCurrentUserId()).FirstOrDefault();
            
            currentUser.UserName = user.UserName;
            currentUser.fk_defaultPlaceToEat = user.fk_defaultPlaceToEat;
            currentUser.fk_defaultPlaceToGetFood = user.fk_defaultPlaceToGetFood;
            currentUser.preferredLunchTime = user.preferredLunchTime;
            ViewBag.isAdmin = _context.UserRoles.Where(u => u.RoleId == user.Id).Any();

            _context.SaveChanges();

            return RedirectToAction("UserIndex");
        }

        [Authorize (Roles = "Admin")]
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

        [Authorize]
        public ActionResult DeleteProfileLunchSession(int id)
        {
            _context.LunchSession.Remove(_context.LunchSession.Where(l=>l.Id == id).First());
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        [Authorize]
        public ActionResult DeleteProfileLunchSessions()
        {
            _context.LunchSession.RemoveRange(_context.LunchSession.Where(l=> l.fk_user == getCurrentUserId()));
            _context.SaveChanges();
        
            return RedirectToAction("Profile");
        }

        //class to get UserId by Name since i dont get the usermanager to give me the Id rn and dont want to perform db actions with data that change (UserName)..
        // TODO: change in case this needs to hold more data as frequent requests might slow down the db
        public string getCurrentUserId()
        {
             var user = _context.User.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            if(user != null)
            {
                return user;
            }
            else 
            {
                return "no user";
            }
        }
    }
}