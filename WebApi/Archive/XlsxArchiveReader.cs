using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WebApi.Domain.Models;

namespace WebApi.Archive;

public class XlsxArchiveReader(Stream stream) : IDisposable
{
    private const int FirstDataRowNum = 4;
    private readonly List<WeatherRecord> _records = [];
    private readonly IWorkbook _workbook = new XSSFWorkbook(stream);

    public void Dispose()
    {
        _workbook.Dispose();
        GC.SuppressFinalize(this);
    }

    public IReadOnlyList<WeatherRecord> Foo()
    {
        _records.Clear();

        for (var sheetIndex = 0; sheetIndex < _workbook.NumberOfSheets; sheetIndex++)
        {
            var sheet = _workbook.GetSheetAt(sheetIndex);

            for (var rowNum = FirstDataRowNum; rowNum < sheet.LastRowNum; rowNum++)
            {
                var row = sheet.GetRow(rowNum);

                if (row is null)
                    continue;

                try
                {
                    var weatherRecord = CreateWeatherRecordFromRow(row);
                    _records.Add(weatherRecord);
                }
                catch (Exception e) when (e is ArgumentException or InvalidOperationException)
                {
                    // move to next for-loop iteration
                }
            }
        }

        return _records.AsReadOnly();
    }

    private static WeatherRecord CreateWeatherRecordFromRow(IRow row)
    {
        var builder = new WeatherRecord.Builder();

        ParseAndMapRequiredData(row, builder);
        ParseAndMapWindData(row, builder);
        ParseAndMapCloudData(row, builder);
        ParseAndMapWeatherActivityData(row, builder);

        return builder.Build();
    }

    private static void ParseAndMapRequiredData(IRow row, WeatherRecord.Builder builder)
    {
        var dateOnlyString = row.GetCell(ColumnIndex.Date)?.ToString();
        var timeOnlyString = row.GetCell(ColumnIndex.Time)?.ToString();

        if (dateOnlyString is null || timeOnlyString is null)
            throw new ArgumentException("Row doesnt contain required data", nameof(row));

        if (DateOnly.TryParseExact(dateOnlyString, "dd.MM.yyyy", out var dateOnly)
            && TimeOnly.TryParseExact(timeOnlyString, "HH:mm", out var timeOnly))
            builder.AtTime(dateOnly.ToDateTime(timeOnly));
        else
            throw new ArgumentException("Unable to parse DateTime", nameof(row));


        var temperatureString = row.GetCell(ColumnIndex.Temperature)?.ToString();
        var humidityString = row.GetCell(ColumnIndex.Humidity)?.ToString();
        var dewPointString = row.GetCell(ColumnIndex.DewPoint)?.ToString();
        var atmospherePressureString = row.GetCell(ColumnIndex.AtmospherePressure)?.ToString();

        if (!float.TryParse(temperatureString, out var temperature)
            || !float.TryParse(humidityString, out var humidity)
            || !float.TryParse(dewPointString, out var dewPoint)
            || !int.TryParse(atmospherePressureString, out var atmospherePressure))
            throw new ArgumentException("Row doesnt contain required data", nameof(row));

        builder.WithTemperature(temperature);
        builder.WithHumidity(humidity);
        builder.WithDewPoint(dewPoint);
        builder.WithPressure(atmospherePressure);
    }

    private static void ParseAndMapWindData(IRow row, WeatherRecord.Builder builder)
    {
        var windSpeedString = row.GetCell(ColumnIndex.WindSpeed)?.ToString();
        if (int.TryParse(windSpeedString, out var windSpeed))
            builder.WithWindSpeed(Convert.ToInt32(windSpeed));

        var windDirectionsRaw = row.GetCell(ColumnIndex.WindDirection)?.ToString();
        if (windDirectionsRaw is null)
            return;

        var windDirections = windDirectionsRaw.Split(",").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        foreach (var windDirection in windDirections)
            builder.WithWindDirection(windDirection);
    }

    private static void ParseAndMapCloudData(IRow row, WeatherRecord.Builder builder)
    {
        var overcastString = row.GetCell(ColumnIndex.Overcast)?.ToString();
        if (int.TryParse(overcastString, out var overcast))
            builder.WithOvercast(overcast);

        var cloudBaseString = row.GetCell(ColumnIndex.CloudBase)?.ToString();
        if (int.TryParse(cloudBaseString, out var cloudBase))
            builder.WithCloudBase(cloudBase);

        var horizontalVisibilityString = row.GetCell(ColumnIndex.HorizontalVisibility)?.ToString();
        if (int.TryParse(horizontalVisibilityString, out var horizontalVisibility))
            builder.WithHorizontalVisibility(horizontalVisibility);
    }

    private static void ParseAndMapWeatherActivityData(IRow row, WeatherRecord.Builder builder)
    {
        var weatherActivity = row.GetCell(ColumnIndex.WeatherActivity)?.ToString();
        if (!string.IsNullOrWhiteSpace(weatherActivity))
            builder.WithActivity(weatherActivity);
    }


    private static class ColumnIndex
    {
        public const int Date = 0;
        public const int Time = 1;
        public const int Temperature = 2;
        public const int Humidity = 3;
        public const int DewPoint = 4;
        public const int AtmospherePressure = 5;
        public const int WindDirection = 6;
        public const int WindSpeed = 7;
        public const int Overcast = 8;
        public const int CloudBase = 9;
        public const int HorizontalVisibility = 10;
        public const int WeatherActivity = 11;
    }
}