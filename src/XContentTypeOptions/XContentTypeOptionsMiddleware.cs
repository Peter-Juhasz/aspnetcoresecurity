using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the X-Content-Type-Options header to all responses.
    /// </summary>
    /// <param name="app"></param>
    public static void UseXContentTypeOptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<XContentTypeOptionsMiddleware>();
    }

    public static IServiceCollection AddXContentTypeOptions(this IServiceCollection services, XContentTypeOptions policy = XContentTypeOptions.NoSniff)
    {
        return services.AddSingleton<XContentTypeOptionsMiddleware>();
    }


    internal sealed class XContentTypeOptionsMiddleware : IMiddleware
    {
        private static readonly StringValues HeaderValue = "nosniff";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;
                response.Headers.XContentTypeOptions = HeaderValue;

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
