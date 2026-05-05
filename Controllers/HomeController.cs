using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitInsight.Data;
using TransitInsight.Models;
using TransitInsight.Services;

namespace TransitInsight.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly EnturService _enturService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        ApplicationDbContext context,
        EnturService enturService,
        ILogger<HomeController> logger)
    {
        _context = context;
        _enturService = enturService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var departures = await _context.Departures
            .Include(d => d.StopPlace)
            .OrderBy(d => d.ExpectedDepartureTime)
            .ToListAsync();

        var chartGroups = departures
            .GroupBy(d => d.TransportMode)
            .OrderBy(g => g.Key)
            .ToList();

        ViewBag.LastUpdated = await _context.ImportLogs
            .OrderByDescending(i => i.ImportedAt)
            .Select(i => i.ImportedAt)
            .FirstOrDefaultAsync();

        var viewModel = new DashboardViewModel
        {
            TotalStopPlaces = await _context.StopPlaces.CountAsync(),
            TotalDepartures = departures.Count,
            DelayedDepartures = departures.Count(d => d.DelayMinutes > 0),
            AverageDelayMinutes = departures.Any()
                ? Math.Round(departures.Average(d => d.DelayMinutes), 1)
                : 0,
            LatestDepartures = departures
                .Take(6)
                .ToList(),
            ChartLabels = chartGroups.Select(g => g.Key).ToList(),
            ChartValues = chartGroups.Select(g => g.Count()).ToList()
        };

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefreshLiveDepartures()
    {
        var stopPlaces = await _context.StopPlaces
            .Where(s => !string.IsNullOrWhiteSpace(s.EnturId))
            .ToListAsync();

        foreach (var stopPlace in stopPlaces)
        {
            var oldDepartures = _context.Departures
                .Where(d => d.StopPlaceId == stopPlace.Id);

            _context.Departures.RemoveRange(oldDepartures);

            var newDepartures = await _enturService.GetDeparturesAsync(
                stopPlace.Id,
                stopPlace.EnturId);

            _context.Departures.AddRange(newDepartures);

            _context.ImportLogs.Add(new ImportLog
            {
                Source = "Entur API",
                NumberOfDepartures = newDepartures.Count,
                Message = $"Live dashboard refresh for {stopPlace.Name}"
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}