// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

using WebApi.Utility;

namespace WebApi.Domain.Models;

public class WeatherRecord
{
    private WeatherRecord()
    {
    }

    public DateTime DateTime { get; private set; }

    public float Temperature { get; private set; }

    public float Humidity { get; private set; }
    public float DewPoint { get; private set; }

    public int AtmospherePressure { get; private set; }

    public int? WindSpeed { get; private set; }
    public ICollection<WindDirection> WindDirections { get; private set; } = new List<WindDirection>();

    public int? Overcast { get; private set; }
    public int? CloudBase { get; private set; }
    public int? HorizontalVisibility { get; private set; }

    public int? WeatherActivityId { get; private set; }
    public WeatherActivity? WeatherActivity { get; private set; }

    public void RefreshForeignKeysUsingContext(WeatherArchiveContext context)
    {
        if (WindDirections.Count > 0)
            WindDirections = WindDirections
                .Select(direction => context.WindDirections.Local
                    .FirstOrDefault(windDirection => windDirection.Name == direction.Name, direction))
                .ToList();

        if (WeatherActivity is null)
            return;

        var localActivity = context.WeatherActivities.Local
            .FirstOrDefault(activity => activity.Name == WeatherActivity.Name);

        if (localActivity is null)
            return;

        WeatherActivityId = localActivity.Id;
        WeatherActivity = null;
    }

    public class Builder
    {
        private const int MinTemperature = -273;
        private readonly WeatherRecord _weatherRecord = new();

        private bool _isDateTimeSet;
        private bool _isDewPointSet;
        private bool _isHumiditySet;
        private bool _isPressureSet;
        private bool _isTemperatureSet;

        public Builder AtTime(DateTime dateTime)
        {
            _isDateTimeSet = true;
            _weatherRecord.DateTime = ArchiveTimeConverter.MoscowToUtc(dateTime);
            return this;
        }

        public Builder WithTemperature(float temperature)
        {
            if (temperature < MinTemperature)
                throw new ArgumentException("Temperature is lower than absolute zero", nameof(temperature));

            _isTemperatureSet = true;
            _weatherRecord.Temperature = temperature;
            return this;
        }

        public Builder WithHumidity(float humidity)
        {
            if (humidity is < 0 or > 100)
                throw new ArgumentException("Humidity must be in range 0-100", nameof(humidity));

            _isHumiditySet = true;
            _weatherRecord.Humidity = humidity;
            return this;
        }

        public Builder WithDewPoint(float dewPoint)
        {
            if (dewPoint < MinTemperature)
                throw new ArgumentException("Dew point is lower than absolute zero", nameof(dewPoint));

            _isDewPointSet = true;
            _weatherRecord.DewPoint = dewPoint;
            return this;
        }

        public Builder WithPressure(int pressure)
        {
            if (pressure < 0)
                throw new ArgumentException("Pressure can't be negative", nameof(pressure));

            _isPressureSet = true;
            _weatherRecord.AtmospherePressure = pressure;
            return this;
        }

        public Builder WithWindSpeed(int windSpeed)
        {
            if (windSpeed < 0)
                throw new ArgumentException("Speed can't be negative", nameof(windSpeed));

            _weatherRecord.WindSpeed = windSpeed;
            return this;
        }

        public Builder WithWindDirection(string direction)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(direction);
            _weatherRecord.WindDirections.Add(WindDirection.Create(direction.Trim().ToUpper()));
            return this;
        }

        public Builder WithOvercast(int overcast)
        {
            if (overcast is < 0 or > 100)
                throw new ArgumentException("Overcast must be in range 0-100", nameof(overcast));

            _weatherRecord.Overcast = overcast;
            return this;
        }

        public Builder WithCloudBase(int cloudBase)
        {
            if (cloudBase < 0)
                throw new ArgumentException("Cloud Base must be positive", nameof(cloudBase));

            _weatherRecord.CloudBase = cloudBase;
            return this;
        }

        public Builder WithHorizontalVisibility(int horizontalVisibility)
        {
            if (horizontalVisibility < 0)
                throw new ArgumentException("Horizontal visibility must be positive", nameof(horizontalVisibility));

            _weatherRecord.HorizontalVisibility = horizontalVisibility;
            return this;
        }

        public Builder WithActivity(string activity)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(activity);
            _weatherRecord.WeatherActivity = WeatherActivity.Create(activity.Trim());
            return this;
        }

        public WeatherRecord Build()
        {
            var areRequiredPropertiesSet = _isDateTimeSet && _isTemperatureSet && _isHumiditySet
                                           && _isDewPointSet && _isPressureSet;

            if (!areRequiredPropertiesSet)
                throw new InvalidOperationException("Not all required properties were set");

            return _weatherRecord;
        }
    }
}