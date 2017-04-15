using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the X-Download-Options header with 'noopen' value to each file download.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseXDownloadOptionsHeader(this IApplicationBuilder app, XDownloadOptions options = XDownloadOptions.NoOpen)
        {
            app.UseMiddleware<XDownloadOptionsMiddleware>();
        }


        internal sealed class XDownloadOptionsMiddleware
        {
            public XDownloadOptionsMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            private readonly RequestDelegate _next;

            public XDownloadOptions Mode { get; } = default(XDownloadOptions);

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentDisposition?.DispositionType?.Equals("attachment", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        response.Headers["X-Download-Options"] = "noopen";
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
