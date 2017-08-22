using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the X-UA-Compatible header to each response with text/html media type.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="mode"></param>
        public static void UseXUACompatible(this IApplicationBuilder app, InternetExplorerCompatibiltyMode mode = InternetExplorerCompatibiltyMode.Edge)
        {
            app.UseMiddleware<XUACompatibleMiddleware>(mode);
        }


        internal sealed class XUACompatibleMiddleware
        {
            public XUACompatibleMiddleware(RequestDelegate next, InternetExplorerCompatibiltyMode mode)
            {
                _next = next;
                Mode = mode;
                _headerValue = ConstructHeaderValue(Mode);
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;

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

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        response.Headers["X-UA-Compatible"] = _headerValue;
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
