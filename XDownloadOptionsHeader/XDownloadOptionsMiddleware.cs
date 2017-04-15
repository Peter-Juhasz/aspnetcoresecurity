using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        public static void UseXDownloadOptions(this IApplicationBuilder app, XDownloadOptions options = XDownloadOptions.NoOpen)
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

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentDisposition?.DispositionType.Equals("attachment", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        response.Headers["X-Download-Options"] = "noopen";
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }

    public enum XDownloadOptions
    {
        NoOpen,
    }
}
