using System.ComponentModel.DataAnnotations;

namespace TransitInsight.Models;

public class StopPlace
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string EnturId { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Locality { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public List<Departure> Departures { get; set; } = new();

    public List<FavoriteStop> FavoriteStops { get; set; } = new();
}