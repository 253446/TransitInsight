using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitInsight.Data;
using TransitInsight.Models;

namespace TransitInsight.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
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
            ChartLabels = chartGroups
                .Select(g => g.Key)
                .ToList(),
            ChartValues = chartGroups
                .Select(g => g.Count())
                .ToList()
        };

        return View(viewModel);
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