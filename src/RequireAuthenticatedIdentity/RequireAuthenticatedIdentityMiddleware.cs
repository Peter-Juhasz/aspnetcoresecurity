using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Requires an authenticated identity, otherwise returns 401 Unauthorized.
        /// </summary>
        /// <param name="app"></param>
        public static void UseRequireAuthenticatedIdentity(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequireAuthenticatedIdentityMiddleware>();
        }


        internal sealed class RequireAuthenticatedIdentityMiddleware : IMiddleware
        {
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                if (!context.User?.Identity.IsAuthenticated ?? false)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                await next.Invoke(context);
            }
        }
    }
}
