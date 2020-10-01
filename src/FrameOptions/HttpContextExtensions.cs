using PeterJuhasz.AspNetCore.Extensions.Security;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static void SetFrameOptions(this HttpContext context, FrameOptionsPolicy policy)
        {
            context.Items[nameof(FrameOptionsPolicy)] = policy;
        }
    }
}
