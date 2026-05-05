using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitInsight.Data;
using TransitInsight.Models;
using TransitInsight.Services;

namespace TransitInsight.Controllers;

public class NearbyStopsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly EnturService _enturService;

    public NearbyStopsController(ApplicationDbContext context, EnturService enturService)
    {
        _context = context;
        _enturService = enturService;
    }

    public IActionResult Index()
    {
        return View(new List<EnturStopSearchResult>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(double? latitude, double? longitude, string? selectedPlace)
    {
        if (!string.IsNullOrWhiteSpace(selectedPlace))
        {
            var placeResults = await _enturService.SearchStopPlacesAsync(selectedPlace);

            var selected = placeResults.FirstOrDefault(s => s.Latitude.HasValue && s.Longitude.HasValue);

            if (selected == null)
            {
                ViewBag.SearchInfo = $"No position found for {selectedPlace}.";
                return View("Index", new List<EnturStopSearchResult>());
            }

            var nearbyFromSelectedPlace = await _enturService.GetNearbyStopsAsync(
                selected.Latitude.Value,
                selected.Longitude.Value);

            ViewBag.SearchInfo = $"Showing stops near selected place: {selectedPlace}";

            return View("Index", nearbyFromSelectedPlace);
        }

        if (!latitude.HasValue || !longitude.HasValue)
        {
            ViewBag.SearchInfo = "No location was provided.";
            return View("Index", new List<EnturStopSearchResult>());
        }

        var nearbyStops = await _enturService.GetNearbyStopsAsync(latitude.Value, longitude.Value);

        ViewBag.SearchInfo = $"Showing stops near browser location: {latitude.Value}, {longitude.Value}";

        return View("Index", nearbyStops);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveStop(string enturId, string name, string? locality, double? latitude, double? longitude)
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

        return RedirectToAction("Index", "StopPlaces");
    }
}