namespace TransitInsight.Models;

public class EnturStopSearchResult
{
    public string EnturId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Locality { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // 🔥 NY: avganger per stopp
    public List<Departure> Departures { get; set; } = new();
}