using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitInsight.Data;
using TransitInsight.Models;

namespace TransitInsight.Controllers;

public class StopPlacesController : Controller
{
    private readonly ApplicationDbContext _context;

    public StopPlacesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var stopPlaces = await _context.StopPlaces
            .Include(s => s.Departures)
            .OrderBy(s => s.Name)
            .ToListAsync();

        return View(stopPlaces);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StopPlace stopPlace)
    {
        if (!ModelState.IsValid)
        {
            return View(stopPlace);
        }

        _context.StopPlaces.Add(stopPlace);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var stopPlace = await _context.StopPlaces
            .Include(s => s.Departures)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stopPlace == null)
        {
            return NotFound();
        }

        return View(stopPlace);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var stopPlace = await _context.StopPlaces.FindAsync(id);

        if (stopPlace == null)
        {
            return NotFound();
        }

        return View(stopPlace);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StopPlace stopPlace)
    {
        if (id != stopPlace.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(stopPlace);
        }

        _context.Update(stopPlace);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var stopPlace = await _context.StopPlaces.FindAsync(id);

        if (stopPlace == null)
        {
            return NotFound();
        }

        return View(stopPlace);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var stopPlace = await _context.StopPlaces.FindAsync(id);

        if (stopPlace != null)
        {
            _context.StopPlaces.Remove(stopPlace);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}