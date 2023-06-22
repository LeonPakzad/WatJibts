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
            return View(_context.User.Find(userId));
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var user = _context.User.Where(u => u.Id == Id).FirstOrDefault();
            // get locations
            // var locationsToEat  = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
            // var locationsToGetFood = _context.Location.ToList();
            // var locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);

            // locationToGetFood.Select(m => new SelectListItem { Text = m, Value = m })
            // ViewBag.LocationToEat = new SelectList(_context.Location.ToList().Where(p => p.isPlaceToEat == true), "Id", "name");
            // ViewBag.LoctionToGetFood = new SelectList(_context.Location.ToList().Where(p => p.isPlaceToEat == true), "Id", "name");
            // return View(user);


            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);
            ViewBag.locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);


            return View( 
                // new UserEdit
                // {
                //     User = _context.User.Where(u => u.Id == Id).FirstOrDefault(), 
                //     LocationsToEat = (List<SelectListItem>)locationsToEat, 
                //     LocationsToGetFood = (List<SelectListItem>)locationsToGetFood 
                // }
            );
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            _context.SaveChanges();
            // if (ModelState.IsValid)
            // {
            //     _context.Update(user);
            //     await _context.SaveChangesAsync();
        
            //     return RedirectToAction(nameof(Index));
            // }
         
            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);
            ViewBag.locationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
            return View(user);


            // if(ModelState.IsValid)
            // {
            //     _context.Update(user);
            //     _context.SaveChanges();
            //     return RedirectToAction("UserIndex");
            // }

            // return View(user);
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