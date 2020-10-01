using Microsoft.AspNetCore.Mvc.Filters;
using PeterJuhasz.AspNetCore.Extensions.Security;

namespace Microsoft.AspNetCore.Builder
{
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

            context.HttpContext.Items[nameof(FrameOptionsPolicy)] = Policy;
        }
    }
}
