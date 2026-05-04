using Microsoft.EntityFrameworkCore;
using TransitInsight.Models;

namespace TransitInsight.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<StopPlace> StopPlaces => Set<StopPlace>();
    public DbSet<Departure> Departures => Set<Departure>();
    public DbSet<FavoriteStop> FavoriteStops => Set<FavoriteStop>();
    public DbSet<ImportLog> ImportLogs => Set<ImportLog>();
}