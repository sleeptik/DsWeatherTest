// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace WebApi.Domain.Models;

public class WindDirection
{
    private WindDirection()
    {
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;

    public static WindDirection Create(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        return new WindDirection { Name = name };
    }
}