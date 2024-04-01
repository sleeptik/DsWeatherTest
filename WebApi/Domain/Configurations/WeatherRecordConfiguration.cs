using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Models;

namespace WebApi.Domain.Configurations;

public class WeatherRecordConfiguration : IEntityTypeConfiguration<WeatherRecord>
{
    public void Configure(EntityTypeBuilder<WeatherRecord> builder)
    {
        builder.HasKey(record => record.DateTime);
        builder.HasMany(record => record.WindDirections).WithMany();
        builder.HasOne(record => record.WeatherActivity).WithMany();
    }
}