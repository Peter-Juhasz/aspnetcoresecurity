using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the X-Content-Type-Options header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="policy"></param>
        public static void UseXContentTypeOptions(this IApplicationBuilder app, XContentTypeOptions policy = XContentTypeOptions.NoSniff)
        {
            app.UseMiddleware<XContentTypeOptionsMiddleware>();
        }


        internal sealed class XContentTypeOptionsMiddleware
        {
            public XContentTypeOptionsMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            private readonly RequestDelegate _next;

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;
                    response.Headers["X-Content-Type-Options"] = "nosniff";

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
