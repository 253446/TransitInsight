namespace TransitInsight.Models;

public class DashboardViewModel
{
    public int TotalStopPlaces { get; set; }
    public int TotalDepartures { get; set; }
    public int DelayedDepartures { get; set; }
    public int OnTimeDepartures { get; set; }
    public double AverageDelayMinutes { get; set; }

    public Departure? NextDeparture { get; set; }
    public string MostActiveStopName { get; set; } = "N/A";
    public int MostActiveStopDepartures { get; set; }

    public List<Departure> LatestDepartures { get; set; } = new();

    public List<string> TransportModeLabels { get; set; } = new();
    public List<int> TransportModeValues { get; set; } = new();

    public List<string> DelayStatusLabels { get; set; } = new();
    public List<int> DelayStatusValues { get; set; } = new();
}