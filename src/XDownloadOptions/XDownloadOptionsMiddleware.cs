using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the X-Download-Options header with 'noopen' value to all file downloads.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseXDownloadOptions(this IApplicationBuilder app, XDownloadOptions options = XDownloadOptions.NoOpen)
        {
            app.UseMiddleware<XDownloadOptionsMiddleware>();
        }


        internal sealed class XDownloadOptionsMiddleware : IMiddleware
        {
            private static readonly StringValues HeaderValue = "noopen";

            public XDownloadOptions Mode { get; } = default;

            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.Headers.TryGetValue(HeaderNames.ContentDisposition, out var values) &&
                        values.Any(v => v.StartsWith("attachment", StringComparison.OrdinalIgnoreCase)))
                    {
                        response.Headers["X-Download-Options"] = HeaderValue;
                    }

                    return Task.CompletedTask;
                });

                await next.Invoke(context);
            }
        }
    }
}
