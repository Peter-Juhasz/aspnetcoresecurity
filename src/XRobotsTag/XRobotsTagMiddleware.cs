using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the X-Robots-Tag header to all responses.
    /// </summary>
    /// <param name="noIndex"></param>
    /// <param name="noFollow"></param>
    public static IServiceCollection AddXRobotsTag(this IServiceCollection services, bool noIndex = true, bool noFollow = true)
    {
        RobotsTagDirectives directives = noIndex
            ? noFollow
                ? RobotsTagDirectives.NoIndex | RobotsTagDirectives.NoFollow
                : RobotsTagDirectives.NoIndex
            : noFollow
                ? RobotsTagDirectives.NoFollow
                : RobotsTagDirectives.All
        ;

        return services.AddXRobotsTag(new RobotsTagDirectiveList
        {
            Directives = directives,
        });
    }

    /// <summary>
    /// Adds the X-Robots-Tag header to all responses.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="directives"></param>
    public static void UseXRobotsTag(this IApplicationBuilder app)
    {
        app.UseMiddleware<XRobotsTagMiddleware>();
    }

    public static IServiceCollection AddXRobotsTag(this IServiceCollection services, RobotsTagDirectiveList options)
    {
        services.AddSingleton<XRobotsTagMiddleware>();
        services.AddSingleton(options);
        return services;
    }


    internal sealed class XRobotsTagMiddleware : IMiddleware
    {
        public XRobotsTagMiddleware(RobotsTagDirectiveList headerValue)
        {
            Directives = headerValue ?? throw new ArgumentNullException(nameof(headerValue));
            _headerValue = Directives.ToString();
        }

        private readonly StringValues _headerValue;

        public RobotsTagDirectiveList Directives { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;
                response.Headers["X-Robots-Tag"] = _headerValue;

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
