using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the Content-Security-Policy header to responses with content type text/html.
    /// </summary>
    /// <param name="app"></param>
    public static void UseContentSecurityPolicy(this IApplicationBuilder app)
    {
        app.UseMiddleware<ContentSecurityPolicyMiddleware>();
    }

    public static IServiceCollection AddContentSecurityPolicy(this IServiceCollection services, CspDirectiveList directives)
    {
        services.AddSingleton<ContentSecurityPolicyMiddleware>();
        services.AddSingleton<CspDirectiveList>(directives);
        return services;
    }

    internal sealed class ContentSecurityPolicyMiddleware : IMiddleware
    {
        public ContentSecurityPolicyMiddleware(IHostEnvironment environment, CspDirectiveList directives)
        {
            Directives = directives ?? throw new ArgumentNullException(nameof(directives));
            _headerValue = Directives.ToString();
            _isDevelopment = environment.IsDevelopment();
        }

        public CspDirectiveList Directives { get; }
        private readonly StringValues _headerValue;
        private readonly bool _isDevelopment;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;

                if (response.Headers.TryGetValue(HeaderNames.ContentType, out var values) &&
                    values.Any(v => v.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)))
                {
                    var options = Directives;
                    var headerValue = _headerValue;

                        // allow inline styles and scripts for developer exception page
                        if (_isDevelopment)
                    {
                        if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
                        {
                            var developerOptions = Directives.Clone();
                            developerOptions.StyleSrc = (developerOptions.StyleSrc ?? StyleCspDirective.Empty).AddUnsafeInline();
                            developerOptions.ScriptSrc = (developerOptions.ScriptSrc ?? ScriptCspDirective.Empty).AddUnsafeInline();
                            options = developerOptions;
                        }
                        headerValue = options.ToString();
                    }

                    response.Headers.ContentSecurityPolicy = headerValue;
                }

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
