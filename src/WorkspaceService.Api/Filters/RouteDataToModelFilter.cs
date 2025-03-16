using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WorkspaceService.Api.Filters;

public class RouteDataToModelFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var routeData = context.RouteData.Values;

        foreach (var param in context.ActionArguments)
        {
            var model = param.Value;
            
            if (model == null || model.GetType().IsPrimitive || model is string)
                continue;

            foreach (var routeValue in routeData)
            {
                var property = model.GetType().GetProperty(routeValue.Key,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(model, property.PropertyType);
                        property.SetValue(model, convertedValue);
                    }
                    catch (Exception ex)
                    {}
                }
            }
        }
    }
    
    public void OnActionExecuted(ActionExecutedContext context) {}
}