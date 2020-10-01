using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;

namespace Microsoft.AspNetCore.Builder
{
    [Obsolete]
    public class FrameOptionsAttribute : ActionFilterAttribute
    {
        public FrameOptionsAttribute(FrameOptionsPolicy policy)
        {
            Policy = policy;
        }

        public FrameOptionsPolicy Policy { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            context.HttpContext.SetFrameOptions(Policy);
        }
    }
}
