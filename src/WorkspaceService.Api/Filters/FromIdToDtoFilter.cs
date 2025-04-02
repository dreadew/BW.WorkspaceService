using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WorkspaceService.Api.Filters;

public class FromIdToDtoFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Items.TryGetValue("FromId", out var userId) || userId == null)
        {
            return;
        }

        foreach (var param in context.ActionArguments)
        {
            var model = param.Value;
            if (model == null || model.GetType().IsPrimitive || model is string)
                continue;

            var property = model.GetType().GetProperty("FromId",
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (property != null && property.CanWrite)
            {
                try
                {
                    var convertedValue = Convert.ChangeType(userId, property.PropertyType);
                    property.SetValue(model, convertedValue);
                }
                catch (Exception ex)
                { }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}