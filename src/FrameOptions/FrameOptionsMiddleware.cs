using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
    /// </summary>
    /// <param name="app"></param>
    [Obsolete]
    public static void UseFrameOptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<FrameOptionsMiddleware>();
    }

    [Obsolete]
    public static IServiceCollection AddFrameOptions(this IServiceCollection services, FrameOptionsDirective options)
    {
        services.AddSingleton<FrameOptionsMiddleware>();
        services.AddSingleton(options);
        return services;
    }

    /// <summary>
    /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
    /// </summary>
    /// <param name="configure"></param>
    [Obsolete]
    public static IServiceCollection AddFrameOptions(this IServiceCollection services, Action<FrameOptionsDirective> configure)
    {
        FrameOptionsDirective options = new FrameOptionsDirective();
        configure(options);
        return services.AddFrameOptions(options);
    }

    /// <summary>
    /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
    /// </summary>
    /// <param name="policy"></param>
    [Obsolete]
    public static IServiceCollection AddFrameOptions(this IServiceCollection services, FrameOptionsPolicy policy = FrameOptionsPolicy.Deny)
    {
        if (policy == FrameOptionsPolicy.AllowFrom)
            throw new ArgumentException("This overload can't be used to configure ALLOW-FROM policy.", nameof(policy));

        return services.AddFrameOptions(new FrameOptionsDirective(policy));
    }

    /// <summary>
    /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
    /// </summary>
    /// <param name="allowFromUri"></param>
    [Obsolete]
    public static IServiceCollection AddFrameOptions(this IServiceCollection services, Uri allowFromUri)
    {
        return services.AddFrameOptions(new FrameOptionsDirective(allowFromUri));
    }


    internal sealed class FrameOptionsMiddleware : IMiddleware
    {
        public FrameOptionsMiddleware(FrameOptionsDirective options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _headerValue = Options.ToString();
        }

        private readonly StringValues _headerValue;

        public FrameOptionsDirective Options { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;

                    // check whether it is applicable
                    if (response.Headers.TryGetValue(HeaderNames.ContentType, out var values) &&
                    values.Any(v => v.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)))
                {
                    var effectiveValue = _headerValue;

                        // check for overwrite
                        if (context.Items.TryGetValue(nameof(FrameOptionsPolicy), out var policy))
                    {
                        effectiveValue = FrameOptionsDirective.ToString((FrameOptionsPolicy)policy);
                    }

                    response.Headers["X-Frame-Options"] = effectiveValue;
                    response.Headers["Frame-Options"] = effectiveValue;
                }

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
