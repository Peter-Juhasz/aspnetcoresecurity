using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;

namespace Microsoft.AspNetCore.Mvc;

public class XUACompatibleAttribute : ActionFilterAttribute
{
    public XUACompatibleAttribute(InternetExplorerCompatibiltyMode mode)
    {
        Mode = mode;
    }

    public InternetExplorerCompatibiltyMode Mode { get; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        context.HttpContext.SetXUACompatible(Mode);
    }
}
