﻿using System.Collections;
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

    private IEnumerable<LunchSession> getTodaysLunchSessions()
    {
        return  _context.LunchSession.Where
                (l => l.lunchTime.Date == DateTime.Today)
                .OrderBy(l => l.participating)
                .ThenByDescending(l => l.lunchTime)
                .ThenByDescending(l => l.fk_eatingPlace)
                .ThenByDescending(l => l.fk_foodPlace)
                .ToList();
    }

    [HttpGet]
    public IActionResult Index()
    {
        IndexModel IndexModel       = new IndexModel();
        LunchSession lunchSession   = new LunchSession();

        // If there is already a lunchsession from the user, fill the data of this lunch session into the form. 
        // If not, fill the form with the user preferred setings 
        if(_context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).Any())
        {
            IndexModel.LunchSession = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).FirstOrDefault();
        }
        else if (_context.User.Where(u => u.Email == User.Identity.Name).Any())
        {
            User currentUser = _context.User.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            string userId = HttpContext.User.Identity.Name;

            if(userId == null) {
                return NotFound();
            }
            
            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

            IndexModel.LunchSession = new LunchSession();
            IndexModel.LunchSession.fk_eatingPlace = currentUser.fk_defaultPlaceToEat;
            IndexModel.LunchSession.fk_foodPlace = currentUser.fk_defaultPlaceToGetFood;

            IndexModel.LunchSession.lunchTime = DateTime.Now.AddHours(currentUser.preferredLunchTime.Value.Hour);
            IndexModel.LunchSession.lunchTime = DateTime.Now.AddMinutes(currentUser.preferredLunchTime.Value.Minute);
        }
        else
        {
            IndexModel.LunchSession = new LunchSession(); 
            IndexModel.LunchSession.lunchTime = DateTime.UtcNow;  
        }

        // get locations
        ViewBag.LocationsToEat      = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
        ViewBag.LocationsToGetFood  = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

        //get grouped lists of todays lunchsessions
        IEnumerable<LunchSession> todaysLunchSessions = getTodaysLunchSessions();

        IndexModel.publicLunchSessions  = IndexModel.groupPublicLunchSessions(todaysLunchSessions.Where(l => l.participating == true).ToList());
        IndexModel.privateLunchSessions = todaysLunchSessions.Where(l => l.participating == false).ToList();

        return View(IndexModel);
    }

    [HttpPost]
    public IActionResult Index(DateTime lunchTime, bool participating, int fk_foodPlace, int fk_eatingPlace)
    {
        IndexModel IndexModel       = new IndexModel();
        LunchSession lunchSession   = new LunchSession();

        bool lunchSessionExists = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).Any();

        // check if user already added a lunchsession, edit the existing, if that is the case. Use preferred Lunchsettings otherwise
        if(lunchSessionExists)
        {
            lunchSession = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).FirstOrDefault();
            lunchSession.participating  = participating;
            lunchSession.fk_foodPlace   = fk_foodPlace;
            lunchSession.fk_eatingPlace = fk_eatingPlace;
            lunchSession.lunchTime      = lunchTime;
        }
        else{
            lunchSession.fk_user        = User.Identity.Name;
            lunchSession.participating  = participating;
            lunchSession.fk_foodPlace   = fk_foodPlace;
            lunchSession.fk_eatingPlace = fk_eatingPlace;
            lunchSession.lunchTime      = lunchTime;
            
            _context.Add(lunchSession);
        }

        _context.SaveChanges();

        //get grouped lists of todays lunchsessions
        IEnumerable<LunchSession> todaysLunchSessions = getTodaysLunchSessions();
        IndexModel.publicLunchSessions  = IndexModel.groupPublicLunchSessions(todaysLunchSessions.Where(l => l.participating == true).ToList());
        IndexModel.privateLunchSessions = todaysLunchSessions.Where(l => l.participating == false).ToList();
        IndexModel.LunchSession = lunchSession;

        // If there is already a lunchsession from the user, fill the data of this lunch session into the form. 
        // If not, fill the form with the user preferred setings 
        if(_context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).Any())
        {
            IndexModel.LunchSession = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).FirstOrDefault();
        }
        else if (_context.User.Where(u => u.Email == User.Identity.Name).Any())
        {
            User currentUser = _context.User.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            string userId = HttpContext.User.Identity.Name;

            if(userId == null) {
                return NotFound();
            }
            
            ViewBag.locationsToGetFood = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

            IndexModel.LunchSession = new LunchSession();
            IndexModel.LunchSession.fk_eatingPlace = currentUser.fk_defaultPlaceToEat;
            IndexModel.LunchSession.fk_foodPlace = currentUser.fk_defaultPlaceToGetFood;

            IndexModel.LunchSession.lunchTime = DateTime.Now.AddHours(currentUser.preferredLunchTime.Value.Hour);
            IndexModel.LunchSession.lunchTime = DateTime.Now.AddMinutes(currentUser.preferredLunchTime.Value.Minute);
        }
        else
        {
            IndexModel.LunchSession = new LunchSession(); 
            IndexModel.LunchSession.lunchTime = DateTime.Now;  
        }
        
        IndexModel.LunchSession     = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).FirstOrDefault();

        // get locations
        ViewBag.LocationsToEat      = _context.Location.ToList().Where(p => p.isPlaceToEat == true);
        ViewBag.LocationsToGetFood  = _context.Location.ToList().Where(p => p.isPlaceToGetFood == true);

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

    public IActionResult JoinSession(int lunchSessionId)
    {
        LunchSession lunchSession = new LunchSession();
        LunchSession joinedSession = _context.LunchSession.Where(l => l.Id == lunchSessionId).FirstOrDefault();

        // check if the user already has a lunch Session
        bool lunchSessionExists = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).Any();
        
        _context.LunchSession.Where
                (l => l.lunchTime.Date == DateTime.Today);

        if(lunchSessionExists)
        {
            lunchSession = _context.LunchSession.Where(l => l.lunchTime.Date == DateTime.Today & l.fk_user == User.Identity.Name).FirstOrDefault();
            lunchSession.participating  = joinedSession.participating;
            lunchSession.fk_foodPlace   = joinedSession.fk_foodPlace;
            lunchSession.fk_eatingPlace = joinedSession.fk_eatingPlace;
            lunchSession.lunchTime      = joinedSession.lunchTime;
        }
        else
        {
            lunchSession.fk_user        = User.Identity.Name;
            lunchSession.participating  = joinedSession.participating;
            lunchSession.fk_foodPlace   = joinedSession.fk_foodPlace;
            lunchSession.fk_eatingPlace = joinedSession.fk_eatingPlace;
            lunchSession.lunchTime      = joinedSession.lunchTime;
            
            _context.Add(lunchSession);
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult clearTodaysLunchSessions()
    {
        _context.LunchSession.RemoveRange(_context.LunchSession.Where
                (l => l.lunchTime.Date == DateTime.Today));
        _context.SaveChanges();
        
        return RedirectToAction("Index");
    }
}
