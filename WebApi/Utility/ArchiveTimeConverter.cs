namespace WebApi.Utility;

public static class ArchiveTimeConverter
{
    private static readonly TimeZoneInfo RussianStandardTime =
        TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

    public static DateTime MoscowToUtc(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, RussianStandardTime);
    }

    public static DateTime UtcToMoscow(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, RussianStandardTime);
    }
}