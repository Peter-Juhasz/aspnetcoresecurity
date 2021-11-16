using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the Feature-Policy header to responses with content type text/html.
    /// </summary>
    /// <param name="app"></param>
    [Obsolete]
    public static void UseFeaturePolicy(this IApplicationBuilder app)
    {
        app.UseMiddleware<FeaturePolicyMiddleware>();
    }

    [Obsolete]
    public static IServiceCollection AddFeaturePolicy(this IServiceCollection services, FeatureDirectiveList options)
    {
        services.AddSingleton<FeaturePolicyMiddleware>();
        services.AddSingleton(options);
        return services;
    }


    internal sealed class FeaturePolicyMiddleware : IMiddleware
    {
        public FeaturePolicyMiddleware(FeatureDirectiveList options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _headerValue = Options.ToString();
        }

        public FeatureDirectiveList Options { get; }
        private readonly StringValues _headerValue;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;

                if (response.Headers.TryGetValue(HeaderNames.ContentType, out var values) &&
                    values.Any(v => v.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)))
                {
                    response.Headers["Feature-Policy"] = _headerValue;
                }

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
