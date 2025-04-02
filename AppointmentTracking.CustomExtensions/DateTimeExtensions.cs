namespace AppointmentTracking.CustomExtensions;

public static class DateTimeExtensions
{
    // Seçilen ay ve hafta için başlangıç tarihini getir
    public static DateTime GetWeekStartDate(this DateTime dateTime, int year, int month, int week)
    {
        var firstDayOfMonth = new DateTime(year, month, 1);
        var firstMonday = firstDayOfMonth.AddDays((8 - (int)firstDayOfMonth.DayOfWeek) % 7);
        var result = firstMonday.AddDays((week - 1) * 7);
        return result;
    }
}
