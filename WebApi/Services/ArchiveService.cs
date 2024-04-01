using Microsoft.EntityFrameworkCore;
using WebApi.Domain;
using WebApi.Domain.Models;
using WebApi.Utility;

namespace WebApi.Services;

public class ArchiveService(WeatherArchiveContext context)
{
    public IDictionary<DateTime, List<WeatherRecord>> GetRecordByYearMonth(DateTime dateTime)
    {
        var endDate = dateTime.AddMonths(1);

        var weatherRecords = context.WeatherRecords.AsNoTracking()
            .Include(record => record.WindDirections)
            .Include(record => record.WeatherActivity)
            .Where(record => record.DateTime >= dateTime && record.DateTime < endDate)
            .OrderBy(record => record.DateTime)
            .ToList();

        var groupedByDay = weatherRecords
            .GroupBy(record => ArchiveTimeConverter.UtcToMoscow(record.DateTime).Date)
            .ToDictionary(
                record => record.Key,
                records => records.ToList()
            );

        return groupedByDay;
    }

    public void SaveRecords(IList<WeatherRecord> records)
    {
        using var transaction = context.Database.BeginTransaction();

        try
        {
            context.WeatherActivities.Load();
            context.WindDirections.Load();

            var existingRecordIds = context.WeatherRecords
                .Select(record => record.DateTime)
                .ToList();

            records = records.ExceptBy(existingRecordIds, record => record.DateTime).ToList();

            var windDirections = records
                .Where(record => record.WindDirections.Count > 0)
                .SelectMany(record => record.WindDirections)
                .DistinctBy(direction => direction.Name)
                .ExceptBy(context.WindDirections.Local.Select(direction => direction.Name), direction => direction.Name)
                .ToList();

            context.WindDirections.AddRange(windDirections);
            context.SaveChanges();

            var activities = records
                .Where(record => record.WeatherActivity != null)
                .Select(record => record.WeatherActivity)
                .OfType<WeatherActivity>()
                .DistinctBy(activity => activity.Name)
                .ExceptBy(context.WeatherActivities.Local.Select(activity => activity.Name), activity => activity.Name)
                .ToList();

            context.WeatherActivities.AddRange(activities);
            context.SaveChanges();

            foreach (var weatherRecord in records)
                weatherRecord.RefreshForeignKeysUsingContext(context);
            
            context.WeatherRecords.AddRange(records);
            context.SaveChanges(false);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }
    }
}