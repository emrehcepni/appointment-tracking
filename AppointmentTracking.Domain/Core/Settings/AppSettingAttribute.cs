namespace AppointmentTracking.Domain.Core.Settings;

public class AppSettingAttribute : Attribute
{
    public string Path { get; set; }
}
