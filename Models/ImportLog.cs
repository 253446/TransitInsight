using System.ComponentModel.DataAnnotations;

namespace TransitInsight.Models;

public class ImportLog
{
    public int Id { get; set; }

    public DateTime ImportedAt { get; set; } = DateTime.Now;

    [Required]
    [StringLength(80)]
    public string Source { get; set; } = "Entur API";

    public int NumberOfDepartures { get; set; }

    [StringLength(200)]
    public string? Message { get; set; }
}
