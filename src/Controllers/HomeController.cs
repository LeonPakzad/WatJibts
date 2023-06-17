using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public IActionResult Index()
    {
        dynamic homeModel = new ExpandoObject();

        // get lunchsessions which were added today
        var today = new DateTime();
        homeModel.LunchSessions = _context.LunchSession.DefaultIfEmpty().ToList().Where(l => l.lunchTime == today);

        // get locations
        homeModel.LocationToEat = _context.Location.Where(p => p.isPlaceToEat == true);
        homeModel.LocationToGetFood = _context.Location.Where(p => p.isPlaceToGetFood == true);

        return View(homeModel);
    }

    [HttpPost]
    public IActionResult Index(LunchSession lunchSession)
    {
        _context.Add(lunchSession);
        _context.SaveChanges();

        dynamic homeModel = new ExpandoObject();
        homeModel.LunchSession = lunchSession;

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
