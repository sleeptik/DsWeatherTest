using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Models;

namespace WebApi.Domain;

public class WeatherArchiveContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<WeatherRecord> WeatherRecords => Set<WeatherRecord>();
    public DbSet<WindDirection> WindDirections => Set<WindDirection>();
    public DbSet<WeatherActivity> WeatherActivities => Set<WeatherActivity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeatherArchiveContext).Assembly);
    }
}