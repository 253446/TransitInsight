using System.ComponentModel.DataAnnotations;

namespace TransitInsight.Models;

public class Departure
{
    public int Id { get; set; }

    [Required]
    public int StopPlaceId { get; set; }

    public StopPlace? StopPlace { get; set; }

    [Required]
    [StringLength(80)]
    public string LineName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Destination { get; set; } = string.Empty;

    public DateTime AimedDepartureTime { get; set; }

    public DateTime ExpectedDepartureTime { get; set; }

    public int DelayMinutes { get; set; }

    [StringLength(40)]
    public string TransportMode { get; set; } = "Unknown";
}