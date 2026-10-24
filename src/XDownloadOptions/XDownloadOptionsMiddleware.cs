using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the X-Download-Options header with 'noopen' value to all file downloads.
    /// </summary>
    /// <param name="app"></param>
    public static void UseXDownloadOptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<XDownloadOptionsMiddleware>();
    }

    public static IServiceCollection AddXDownloadOptions(this IServiceCollection services, XDownloadOptions options = XDownloadOptions.NoOpen)
    {
        return services.AddSingleton<XDownloadOptionsMiddleware>();
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
                    IsAttachment(values))
                {
                    response.Headers["X-Download-Options"] = HeaderValue;
                }

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }

        private static bool IsAttachment(StringValues values)
        {
            foreach (var value in values)
            {
                if (value?.StartsWith("attachment", StringComparison.OrdinalIgnoreCase) is true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
