using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitInsight.Data;
using TransitInsight.Models;
using TransitInsight.Services;

namespace TransitInsight.Controllers;

[Authorize]
public class StopPlacesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly EnturService _enturService;

    public StopPlacesController(ApplicationDbContext context, EnturService enturService)
    {
        _context = context;
        _enturService = enturService;
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
            .Include(s => s.Departures.OrderBy(d => d.ExpectedDepartureTime))
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
        var stopPlace = await _context.StopPlaces
            .Include(s => s.Departures)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stopPlace != null)
        {
            _context.Departures.RemoveRange(stopPlace.Departures);
            _context.StopPlaces.Remove(stopPlace);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportDepartures(int id)
    {
        var stopPlace = await _context.StopPlaces.FindAsync(id);

        if (stopPlace == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(stopPlace.EnturId))
        {
            return BadRequest("EnturId is missing for this stop place.");
        }

        var departures = await _enturService.GetDeparturesAsync(stopPlace.Id, stopPlace.EnturId);

        var oldDepartures = _context.Departures
            .Where(d => d.StopPlaceId == stopPlace.Id);

        _context.Departures.RemoveRange(oldDepartures);
        _context.Departures.AddRange(departures);

        _context.ImportLogs.Add(new ImportLog
        {
            Source = "Entur API",
            NumberOfDepartures = departures.Count,
            Message = $"Imported departures for {stopPlace.Name}"
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = stopPlace.Id });
    }
    public IActionResult SearchEntur()
{
    return View(new List<EnturStopSearchResult>());
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SearchEntur(string searchText)
{
    if (string.IsNullOrWhiteSpace(searchText))
    {
        return View(new List<EnturStopSearchResult>());
    }

    var results = await _enturService.SearchStopPlacesAsync(searchText);

    ViewBag.SearchText = searchText;

    return View(results);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SaveFromEntur(string enturId, string name, string? locality, double? latitude, double? longitude)
{
    var exists = await _context.StopPlaces.AnyAsync(s => s.EnturId == enturId);

    if (!exists)
    {
        _context.StopPlaces.Add(new StopPlace
        {
            EnturId = enturId,
            Name = name,
            Locality = locality,
            Latitude = latitude,
            Longitude = longitude
        });

        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}
}