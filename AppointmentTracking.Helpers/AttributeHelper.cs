using AppointmentTracking.Domain.Core.Settings;
using System.Reflection;

namespace AppointmentTracking.Helpers;

public static class AttributeHelper<TAttribute, TObj> where TAttribute : Attribute where TObj : ISettingsBase
{
    public static TType? GetAttributeValue<TType>(string propertyName) where TType : class
    {
        var instance = Activator.CreateInstance(typeof(TObj));

        if (instance is null)
            return default(TType);

        Type type = instance.GetType();
        if (type is null)
            return default(TType);

        var attribute = type.GetCustomAttribute<TAttribute>();
        if (attribute is null)
            return null;

        var property = attribute.GetType().GetProperty(propertyName);
        if (property is null)
            return default(TType);

        var saltValue = property.GetValue(attribute);
        if (saltValue is null)
            return default(TType);

        var value = Convert.ChangeType(saltValue, typeof(TType));
        return value is null ? default(TType) : (TType)value;
    }
}
