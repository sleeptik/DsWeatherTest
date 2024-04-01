// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace WebApi.Domain.Models;

public class WeatherActivity
{
    private WeatherActivity()
    {
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;

    public static WeatherActivity Create(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        return new WeatherActivity { Name = name };
    }
}