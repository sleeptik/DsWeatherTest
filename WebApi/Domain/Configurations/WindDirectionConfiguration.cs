using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Models;

namespace WebApi.Domain.Configurations;

public class WindDirectionConfiguration : IEntityTypeConfiguration<WindDirection>
{
    public void Configure(EntityTypeBuilder<WindDirection> builder)
    {
        builder.HasAlternateKey(direction => direction.Name);
        builder.Property(direction => direction.Name).HasMaxLength(16);
    }
}