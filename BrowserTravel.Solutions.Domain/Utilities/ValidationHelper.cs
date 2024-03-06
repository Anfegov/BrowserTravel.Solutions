using BrowserTravel.Solutions.Domain.Exceptions;
using System.Reflection;

namespace BrowserTravel.Solutions.Domain.Utilities;

public static class ValidationHelper
{
    public static string ValidateEmptyProperty<T>(T obj, string propertyName)
    {
        object value = string.Empty;
        var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
        {
            throw new CoreException(DomainErrors.Errors.GenericError.NotFoundProperty(propertyName, typeof(T).Name));
        }

        value = property.GetValue(obj);

        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            throw new CoreException(DomainErrors.Errors.GenericError.IsNullOrWhiteSpace(propertyName));
        }

        return value.ToString();
    }
}
