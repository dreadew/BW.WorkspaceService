using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WorkspaceService.Domain.Utils;

public static class ValidationHelper
{
    public static string GetDisplayName(Type type, string propertyName)
    {
        var prop = type.GetProperty(propertyName);
        var displayAttr = prop?.GetCustomAttribute<DisplayAttribute>();
        return displayAttr?.Name ?? propertyName;
    }
}