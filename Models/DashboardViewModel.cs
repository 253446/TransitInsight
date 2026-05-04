namespace TransitInsight.Models;

public class DashboardViewModel
{
    public int TotalStopPlaces { get; set; }
    public int TotalDepartures { get; set; }
    public int DelayedDepartures { get; set; }
    public double AverageDelayMinutes { get; set; }

    public List<Departure> LatestDepartures { get; set; } = new();

    public List<string> ChartLabels { get; set; } = new();
    public List<int> ChartValues { get; set; } = new();
}