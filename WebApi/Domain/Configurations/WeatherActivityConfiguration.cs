using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Models;

namespace WebApi.Domain.Configurations;

public class WeatherActivityConfiguration : IEntityTypeConfiguration<WeatherActivity>
{
    public void Configure(EntityTypeBuilder<WeatherActivity> builder)
    {
        builder.HasAlternateKey(activity => activity.Name);
        builder.Property(activity => activity.Name).HasMaxLength(256);
    }
}