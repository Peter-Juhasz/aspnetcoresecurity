using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public record class XUACompatibleOptions(InternetExplorerCompatibiltyMode Mode);

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the X-UA-Compatible header to each response with text/html media type.
    /// </summary>
    /// <param name="app"></param>
    public static void UseXUACompatible(this IApplicationBuilder app)
    {
        app.UseMiddleware<XUACompatibleMiddleware>();
    }

    public static IServiceCollection AddXUACompatible(this IServiceCollection services, XUACompatibleOptions options)
    {
        services.AddSingleton<XUACompatibleMiddleware>();
        services.AddSingleton(options);
        return services;
    }

    public static IServiceCollection AddXUACompatible(this IServiceCollection services, InternetExplorerCompatibiltyMode mode = InternetExplorerCompatibiltyMode.Edge)
    {
        return services.AddXUACompatible(new XUACompatibleOptions(mode));
    }


    internal sealed class XUACompatibleMiddleware : IMiddleware
    {
        public XUACompatibleMiddleware(XUACompatibleOptions options)
        {
            Mode = options.Mode;
            _headerValue = ConstructHeaderValue(Mode);
        }

        private readonly StringValues _headerValue;

        public InternetExplorerCompatibiltyMode Mode { get; }

        private static readonly IReadOnlyDictionary<InternetExplorerCompatibiltyMode, string> HeaderValues = new Dictionary<InternetExplorerCompatibiltyMode, string>
            {
                { InternetExplorerCompatibiltyMode.Edge, "Edge" },
                { InternetExplorerCompatibiltyMode.IE5, "5" },
                { InternetExplorerCompatibiltyMode.IE7, "7" },
                { InternetExplorerCompatibiltyMode.IE8, "8" },
                { InternetExplorerCompatibiltyMode.IE9, "9" },
                { InternetExplorerCompatibiltyMode.IE10, "10" },
                { InternetExplorerCompatibiltyMode.IE11, "11" },
                { InternetExplorerCompatibiltyMode.EmulateIE7, "EmulateIE7" },
                { InternetExplorerCompatibiltyMode.EmulateIE8, "EmulateIE8" },
                { InternetExplorerCompatibiltyMode.EmulateIE9, "EmulateIE9" },
                { InternetExplorerCompatibiltyMode.EmulateIE10, "EmulateIE10" },
                { InternetExplorerCompatibiltyMode.EmulateIE11, "EmulateIE11" },
            };

        internal static string ConstructHeaderValue(InternetExplorerCompatibiltyMode mode)
        {
            try
            {
                return $"IE={HeaderValues[mode]}";
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException($"Mode '{mode}' not supported.");
            }
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;

                if (response.Headers.TryGetValue(HeaderNames.ContentType, out var values) &&
                    values.Any(v => v.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)))
                {
                    var effectiveValue = _headerValue;

                    if (context.Items.TryGetValue(nameof(InternetExplorerCompatibiltyMode), out var mode))
                    {
                        effectiveValue = ConstructHeaderValue((InternetExplorerCompatibiltyMode)mode);
                    }

                    response.Headers.XUACompatible = effectiveValue;
                }

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
