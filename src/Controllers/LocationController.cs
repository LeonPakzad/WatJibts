
using System.Data.Entity;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;
namespace src.Controllers;

public class LocationController : Controller
{
    private readonly WatDbContext _context;

    public LocationController(WatDbContext context)
    {
        _context = context;
    }

    [Authorize]
    public ActionResult LocationIndex(string error = null)
    {
        if(error != null)
        {
            ViewBag.error = error;
        }

        return View(_context.Location.ToList());
    }

    [HttpGet]
    [Authorize]
    public ActionResult Add()
    {
        var location = new Location();
        return View(location);
    }

    [HttpPost]
    [Authorize]
    public ActionResult Add(Location location)
    {
        _context.Add(location);
        _context.SaveChanges();
        return View("LocationIndex", _context.Location.ToList());
    }

    [Authorize]
    public async Task<IActionResult> LocationById(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var location = await _context.Location
            .FirstOrDefaultAsync(l => l.Id == id);
        if (location == null)
        { 
            return NotFound();
        }

        return View(location);
    }

    [Authorize]
    public ActionResult Edit(int Id)
    {
        var location = _context.Location.Where(l => l.Id == Id).FirstOrDefault();
        return View(location);
    }

    [HttpPost]
    [Authorize]
    public ActionResult Edit(Location location)
    {
        var oldLocation = _context.Location.Where(l => l.Id == location.Id).FirstOrDefault();
        _context.Location.Remove(oldLocation);
        _context.Location.Add(location);
        _context.SaveChanges();

        return RedirectToAction("LocationIndex");
    }

    [Authorize]
    public async Task<IActionResult> LocationsForFood(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var location = await _context.Location
            .FirstOrDefaultAsync(l => l.Id == id);
        if (location == null)
        { 
            return NotFound();
        }

        return View(location);
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
        var location = _context.Location.Find(id);
        
        if (location == null)
        {
            return NotFound();
        }

        //check if location is already in use, redirect if its the case. ToDo: error message
        if(_context.LunchSession.Where(l => l.fk_foodPlace == id || l.fk_eatingPlace == id ).Any())
        {
            return RedirectToAction("LocationIndex", new {error = "error: cant delete a location that is already in use, delete associated lunchsessions first"});    
        }
        
        _context.Location.Remove(location);
        _context.SaveChanges();

        return RedirectToAction("LocationIndex");
    }
}