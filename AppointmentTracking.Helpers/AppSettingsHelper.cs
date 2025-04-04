using AppointmentTracking.Domain.Core.Settings;
using Microsoft.Extensions.Configuration;

namespace AppointmentTracking.Helpers;

public static class AppSettingsHelper
{
    private static IConfiguration? _configuration;

    public static void AppSettingsHelperConfigure(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static TObj? GetData<TObj>() where TObj : ISettingsBase
    {
        if (_configuration is null)
            return default(TObj);

        var path = AttributeHelper<AppSettingAttribute, TObj>.GetAttributeValue<string>("Path");
        if (path is null)
            return default(TObj);

        var section = _configuration.GetSection(path);
        if (section is null)
            return default(TObj);

        var result = section.Get<TObj>();
        return result;
    }

    public static TList? GetDataByList<TList, TBaseType>() where TList : IEnumerable<ISettingsBase>
        where TBaseType : ISettingsBase
    {
        if (_configuration is null)
            return default(TList);

        var path = AttributeHelper<AppSettingAttribute, TBaseType>.GetAttributeValue<string>("Path");
        if (path is null)
            return default(TList);

        var section = _configuration.GetSection(path);
        if (section is null)
            return default(TList);

        var result = section.Get<TList>();
        return result;
    }
}
