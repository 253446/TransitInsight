using System.ComponentModel.DataAnnotations;

namespace TransitInsight.Models;

public class FavoriteStop
{
    public int Id { get; set; }

    [Required]
    public int StopPlaceId { get; set; }

    public StopPlace? StopPlace { get; set; }

    [Required]
    [StringLength(80)]
    public string Nickname { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Notes { get; set; }
}