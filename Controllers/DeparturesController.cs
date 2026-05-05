using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransitInsight.Data;
using TransitInsight.Models;

namespace TransitInsight.Controllers;

[Authorize]
public class DeparturesController : Controller
{
    private readonly ApplicationDbContext _context;

    public DeparturesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var departures = await _context.Departures
            .Include(d => d.StopPlace)
            .OrderBy(d => d.ExpectedDepartureTime)
            .ToListAsync();

        return View(departures);
    }

    public IActionResult Create()
    {
        ViewData["StopPlaceId"] = new SelectList(_context.StopPlaces.OrderBy(s => s.Name), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Departure departure)
    {
        if (!ModelState.IsValid)
        {
            ViewData["StopPlaceId"] = new SelectList(_context.StopPlaces.OrderBy(s => s.Name), "Id", "Name", departure.StopPlaceId);
            return View(departure);
        }

        _context.Departures.Add(departure);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var departure = await _context.Departures
            .Include(d => d.StopPlace)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (departure == null)
        {
            return NotFound();
        }

        return View(departure);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var departure = await _context.Departures.FindAsync(id);

        if (departure == null)
        {
            return NotFound();
        }

        ViewData["StopPlaceId"] = new SelectList(_context.StopPlaces.OrderBy(s => s.Name), "Id", "Name", departure.StopPlaceId);
        return View(departure);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Departure departure)
    {
        if (id != departure.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["StopPlaceId"] = new SelectList(_context.StopPlaces.OrderBy(s => s.Name), "Id", "Name", departure.StopPlaceId);
            return View(departure);
        }

        _context.Update(departure);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var departure = await _context.Departures
            .Include(d => d.StopPlace)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (departure == null)
        {
            return NotFound();
        }

        return View(departure);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var departure = await _context.Departures.FindAsync(id);

        if (departure != null)
        {
            _context.Departures.Remove(departure);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}