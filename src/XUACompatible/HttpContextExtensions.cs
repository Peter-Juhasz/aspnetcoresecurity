using PeterJuhasz.AspNetCore.Extensions.Security;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static void SetXUACompatible(this HttpContext context, InternetExplorerCompatibiltyMode mode)
        {
            context.Items[nameof(InternetExplorerCompatibiltyMode)] = mode;
        }
    }
}
