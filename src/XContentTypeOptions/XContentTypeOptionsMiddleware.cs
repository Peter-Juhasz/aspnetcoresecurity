using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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


        internal sealed class XContentTypeOptionsMiddleware : IMiddleware
        {
            private static readonly StringValues HeaderValue = "nosniff";

            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;
                    response.Headers["X-Content-Type-Options"] = HeaderValue;

                    return Task.CompletedTask;
                });

                await next.Invoke(context);
            }
        }
    }
}
