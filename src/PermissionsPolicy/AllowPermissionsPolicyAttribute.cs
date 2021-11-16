using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.AspNetCore.Mvc;

public class AllowPermissionsPolicyAttribute : ActionFilterAttribute
{
    public AllowPermissionsPolicyAttribute(string feature, string value)
    {
        Feature = feature;
        Value = value;
    }

    public string Feature { get; }
    public string Value { get; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        context.HttpContext.AllowPermissionsPolicy(Feature, Value);
    }
}
