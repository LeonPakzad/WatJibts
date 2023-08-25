using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using src.Data;
using src.Models;
using Microsoft.AspNetCore.Identity;

namespace src.Controllers;

public class HomeController : Controller
{
    private readonly WatDbContext _context;

    public HomeController(WatDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(10)});

        return LocalRedirect(returnUrl);
    }

    private IEnumerable<LunchSessionModel> getTodaysLunchSessions()
    {
        List<LunchSessionModel> todaysLunchSessions = new List<LunchSessionModel>();
        List<User> userList = new List<User>();
        var currentWeekday = (int)DateTime.Now.DayOfWeek;

        var tmpLunchSessions = _context.LunchSession
            .Where(l => l.lunchTime.Date == DateTime.Today)
            .OrderBy(l => l.participating)
            .ThenByDescending(l => l.lunchTime)
            .ThenByDescending(l => l.fk_eatingPlace)
            .ThenByDescending(l => l.fk_foodPlace)
            .ToList();
            
        // get default lunchSessions
        var defaultLunchSessions = _context.LunchSession
            .Where(l => l.isDefault == true & l.weekday == currentWeekday)
            .ToList();

        tmpLunchSessions = tmpLunchSessions.Union(defaultLunchSessions).OrderBy(x => x.Id).ToList();

        // delete dublicates
        foreach(LunchSession tmpLunchSession in tmpLunchSessions.ToList())
        {
            if(tmpLunchSession.isDefault)
            {
                if(tmpLunchSessions.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == tmpLunchSession.fk_user && l.isDefault == false).Any())
                {
                    tmpLunchSessions.Remove(tmpLunchSession);
                }
            }
        }

        // build each lunchsessionmodel
        foreach(LunchSession tmpLunchSession in tmpLunchSessions)
        {
            if(_context.User.Where(u => u.Id == tmpLunchSession.fk_user).Any())
            {
                User tmpUser = _context.User.Where(u => u.Id == tmpLunchSession.fk_user).FirstOrDefault();

                LunchSessionModel todaysLunchSessionModel = new LunchSessionModel
                {
                    Id = tmpLunchSession.Id,
                    lunchTime = tmpLunchSession.lunchTime,
                    participating = tmpLunchSession.participating,
                    fk_foodPlace = tmpLunchSession.fk_foodPlace,
                    fk_eatingPlace = tmpLunchSession.fk_eatingPlace,
                    fk_user = tmpUser.Id,
                    userName = tmpUser.UserName
                };

                if (todaysLunchSessionModel.fk_eatingPlace != -1)
                {
                    todaysLunchSessionModel.eatingPlace = _context.Location.Where(l => l.Id == todaysLunchSessionModel.fk_eatingPlace).FirstOrDefault().name;
                }

                if(todaysLunchSessionModel.fk_foodPlace != -1)
                {
                    todaysLunchSessionModel.foodPlace = _context.Location.Where(l => l.Id == todaysLunchSessionModel.fk_foodPlace).FirstOrDefault().name;
                }

                todaysLunchSessions.Add(todaysLunchSessionModel);
            }
        }
        
        return todaysLunchSessions;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        HomeIndexModel HomeIndexModel   = new HomeIndexModel();
        LunchSession lunchSession       = new LunchSession();

        // If there is already a lunchsession from the user, fill the data of this lunch session into the form. 
        // If not, fill the form with the user preferred setings 
        if(_context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today && l.fk_user == getCurrentUserId() && l.isDefault == false).Any())
        {
            HomeIndexModel.LunchSession = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today && l.fk_user == getCurrentUserId() && l.isDefault == false).FirstOrDefault();
        }
        else if (_context.User.Where(u => u.Id == getCurrentUserId()).Any())
        {
            User currentUser = _context.User.Where(u => u.Id == getCurrentUserId()).FirstOrDefault();
            
            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

            HomeIndexModel.LunchSession = new LunchSession();
            HomeIndexModel.LunchSession.fk_eatingPlace = currentUser.fk_defaultPlaceToEat;
            HomeIndexModel.LunchSession.fk_foodPlace = currentUser.fk_defaultPlaceToGetFood;

            HomeIndexModel.LunchSession.lunchTime = DateTime.Now.AddHours(currentUser.preferredLunchTime.Value.Hour);
            HomeIndexModel.LunchSession.lunchTime = DateTime.Now.AddMinutes(currentUser.preferredLunchTime.Value.Minute);
        }
        else
        {
            HomeIndexModel.LunchSession = new LunchSession(); 
            HomeIndexModel.LunchSession.lunchTime = DateTime.UtcNow;  
        }

        // get locations
        ViewBag.LocationsToEat      = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
        ViewBag.LocationsToGetFood  = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

        //get grouped lists of todays lunchsessions
        IEnumerable<LunchSessionModel> todaysLunchSessions = getTodaysLunchSessions();

        if(todaysLunchSessions != null)
        {
            HomeIndexModel.publicLunchSessions  = HomeIndexModel.
            groupPublicLunchSessions(todaysLunchSessions.Where(l => l.participating == true).
            ToList());
        }

        HomeIndexModel.privateLunchSessions = todaysLunchSessions.Where(l => l.participating == false).ToList();

        return View(HomeIndexModel);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Index(DateTime lunchTime, bool participating, int fk_foodPlace, int fk_eatingPlace)
    {
        LunchSession lunchSession   = new LunchSession();

        bool lunchSessionExists = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == getCurrentUserId()).Any();

        // check if user already added a lunchsession, edit the existing, if that is the case. Use preferred Lunchsettings otherwise
        if(lunchSessionExists)
        {
            lunchSession = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == getCurrentUserId()).FirstOrDefault();
            lunchSession.participating  = participating;
            lunchSession.fk_foodPlace   = fk_foodPlace;
            lunchSession.fk_eatingPlace = fk_eatingPlace;
            lunchSession.lunchTime      = lunchTime;
            lunchSession.isDefault = false;
        }
        else{
            lunchSession.fk_user        = getCurrentUserId();
            lunchSession.participating  = participating;
            lunchSession.fk_foodPlace   = fk_foodPlace;
            lunchSession.fk_eatingPlace = fk_eatingPlace;
            lunchSession.lunchTime      = lunchTime;
            lunchSession.isDefault = false;
            
            _context.Add(lunchSession);
        }

        _context.SaveChanges();

        return RedirectToAction("Index");
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

    [Authorize]
    public IActionResult JoinSession(int lunchSessionId)
    {
        LunchSession lunchSession = new LunchSession();
        LunchSession joinedSession = _context.LunchSession.Where(l => l.Id == lunchSessionId).FirstOrDefault();

        // check if the user already has a lunch Session
        bool lunchSessionExists = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == getCurrentUserId()).Any();
        
        _context.LunchSession.Where
                (l => l.lunchTime.Date == DateTime.Today);

        if(lunchSessionExists)
        {
            lunchSession = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == getCurrentUserId()).FirstOrDefault();
            lunchSession.participating  = joinedSession.participating;
            lunchSession.fk_foodPlace   = joinedSession.fk_foodPlace;
            lunchSession.fk_eatingPlace = joinedSession.fk_eatingPlace;
            lunchSession.lunchTime      = joinedSession.lunchTime;
        }
        else
        {
            lunchSession.fk_user        = getCurrentUserId();
            lunchSession.participating  = joinedSession.participating;
            lunchSession.fk_foodPlace   = joinedSession.fk_foodPlace;
            lunchSession.fk_eatingPlace = joinedSession.fk_eatingPlace;
            lunchSession.lunchTime      = joinedSession.lunchTime;
            
            _context.Add(lunchSession);
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [Authorize (Roles = "Admin")]
    public IActionResult clearTodaysLunchSessions()
    {
        _context.LunchSession.RemoveRange(_context.LunchSession.Where
                (l => l.lunchTime.Date == DateTime.Today && l.isDefault == false));
        _context.SaveChanges();
        
        return RedirectToAction("Index");
    }

    //class to get UserId by Name since i dont get the usermanager to give me the Id rn and dont want to perform db actions with data that change (UserName)
    public string getCurrentUserId()
    {
        if(_context.User.Where(u => u.UserName == User.Identity.Name).Any())
        {
            return _context.User.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
        }
        else 
        {
            return "error";
        }

    }
}