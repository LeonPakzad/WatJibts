
using System.Data.Entity;
using System.Diagnostics;
using System.Dynamic;
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

    public ActionResult LocationIndex()
    {
        return View(_context.Location.ToList());
    }

    [HttpGet]
    public ActionResult Add()
    {
        var location = new Location();
        return View(location);
    }

    [HttpPost]
    public ActionResult Add(Location location)
    {
        _context.Add(location);
        _context.SaveChanges();
        return View(location);
    }

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

    [HttpGet]
    public ActionResult Edit(int Id)
    {
        var location = _context.Location.Where(l => l.Id == Id).FirstOrDefault();
        return View(location);
    }

    [HttpPost]
    public ActionResult Edit(Location location)
    {
        _context.SaveChanges();
        return View(location);
    }


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

    public ActionResult Delete(int id)
    {
        var location = _context.Location.Find(id);
        if (location == null)
        {
            return NotFound();
        }
        
        _context.Location.Remove(location);
        _context.SaveChanges();

        return RedirectToAction("LocationIndex");
    }
}