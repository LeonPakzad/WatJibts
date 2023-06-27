using System.Collections;
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
        IndexModel IndexModel = new IndexModel();

        // get a ordered lunchsessions which were added today
        IEnumerable<LunchSession> todaysLunchSessions = _context.LunchSession.Where
            (l => l.lunchTime.Date == DateTime.Today)
            .OrderBy(l => l.participating)
            .ThenByDescending(l => l.lunchTime)
            .ThenByDescending(l => l.fk_eatingPlace)
            .ThenByDescending(l => l.fk_foodPlace)
            .ToList();

        IndexModel.publicLunchSessions = IndexModel.groupPublicLunchSessions(todaysLunchSessions.Where(l => l.participating == true).ToList());
        IndexModel.privateLunchSessions = todaysLunchSessions.Where(l => l.participating == false).ToList();

        // get locations
        ViewBag.LocationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
        ViewBag.LocationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

        return View(IndexModel);
    }

    [HttpPost]
    public IActionResult Index(DateTime lunchTime, bool participating, int fk_foodPlace, int fk_eatingPlace)
    {
        IndexModel IndexModel = new IndexModel();
        LunchSession lunchSession = new LunchSession();

        // save new lunchsession
        lunchSession.fk_user = HttpContext.User.Identity.Name;
        lunchSession.participating = participating;
        lunchSession.fk_foodPlace = fk_foodPlace;
        lunchSession.fk_eatingPlace = fk_eatingPlace;
        lunchSession.lunchTime = lunchTime;

        _context.Add(lunchSession);
        _context.SaveChanges();

        // get a ordered lunchsessions which were added today
        IEnumerable<LunchSession> todaysLunchSessions = _context.LunchSession.Where
            (l => l.lunchTime.Date == DateTime.Today)
            .OrderBy(l => l.participating)
            .ThenByDescending(l => l.lunchTime)
            .ThenByDescending(l => l.fk_eatingPlace)
            .ThenByDescending(l => l.fk_foodPlace)
            .ToList();

        IndexModel.publicLunchSessions = IndexModel.groupPublicLunchSessions(todaysLunchSessions.Where(l => l.participating == true).ToList());
        IndexModel.privateLunchSessions = todaysLunchSessions.Where(l => l.participating == false).ToList();

        // get locations
        ViewBag.LocationsToEat = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
        ViewBag.LocationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

        return View(IndexModel);
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
