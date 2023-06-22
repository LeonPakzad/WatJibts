using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using src.Data;
using src.Models;

namespace src.Controllers;

public class HomeController : Controller
{
    private readonly WatDbContext _context;

    public HomeController(WatDbContext context)
    {
        _context = context;
    }


    // todo: doublicated code 
    [HttpGet]
    public IActionResult Index()
    {
        dynamic homeModel = new ExpandoObject();

        // get lunchsessions which were added today
        var today = new DateTime();

        var lunchSessions = _context.LunchSession.DefaultIfEmpty().ToList().Where(l => System.Data.Entity.DbFunctions.TruncateTime(l.lunchTime) == DateTime.Today);

        ViewBag.LocationToEat = _context.LunchSession.DefaultIfEmpty().ToList().Where(l => l.lunchTime == today);
        if(lunchSessions == null)
        {
            homeModel.hasLunchSessions = true;
            homeModel.LunchSessions = _context.LunchSession.DefaultIfEmpty().ToList().Where(l => l.lunchTime == today);
        }
        {
            homeModel.hasLunchSessions = false;
        }

        // get locations
        homeModel.LocationToEat = new SelectList(_context.Location.ToList().Where(p => p.isPlaceToEat == true));
        homeModel.LocationToGetFood = new SelectList(_context.Location.ToList().Where(p => p.isPlaceToGetFood == true));

        return View(homeModel);
    }

    [HttpPost]
    public IActionResult Index(bool participating, int placeForFood, int placeToEat, DateTime lunchTime)
    {
 
        LunchSession lunchSession = new LunchSession();
        // save new lunchsession
        lunchSession.fk_user = HttpContext.User.Identity.Name;
        lunchSession.participating = participating;
        lunchSession.fk_foodPlace = placeForFood;
        lunchSession.fk_eatingPlace = placeToEat;
        lunchSession.lunchTime = lunchTime;

        _context.Add(lunchSession);
        _context.SaveChanges();


        dynamic homeModel = new ExpandoObject();
        
        // get lunchsessions which were added today
        var today = new DateTime();
        var lunchSessions = _context.LunchSession.DefaultIfEmpty().ToList().Where(l => System.Data.Entity.DbFunctions.TruncateTime(l.lunchTime) == DateTime.Today);

        if(lunchSessions != null)
        {
            homeModel.hasLunchSessions = true;
            ViewBag.LocationToEat = _context.LunchSession.DefaultIfEmpty().ToList().Where(l => l.lunchTime == today);
        }
        {
            homeModel.hasLunchSessions = false;
        }


        // get locations
        homeModel.LocationToEat = _context.Location.Where(p => p.isPlaceToEat == true);
        homeModel.LocationToGetFood = _context.Location.Where(p => p.isPlaceToGetFood == true);

        return View(homeModel);
    }

    public IActionResult Impressum()
    {
        return View();
    }
    
    [Route("error")]
    [Route("404")]
    public IActionResult PageNotFound()
    {
        string? originalPath = "unknown";
        if (HttpContext.Items.ContainsKey("originalPath"))
        {
            originalPath = HttpContext.Items["originalPath"] as string;
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
