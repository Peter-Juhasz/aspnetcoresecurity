using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the X-XSS-Protection header to each response with text/html media type.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        [Obsolete]
        public static void UseXXSSProtection(this IApplicationBuilder app, XXssProtectionOptions options)
        {
            app.UseMiddleware<XXSSProtectionMiddleware>(options);
        }

        /// <summary>
        /// Adds the X-XSS-Protection header to each response with text/html media type.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="enabled">Enables XSS protection.</param>
        /// <param name="block">Sets Block mode.</param>
        /// <param name="reportUri">Sets the URI the browser is going to report violations to.</param>
        [Obsolete]
        public static void UseXXSSProtection(
            this IApplicationBuilder app,
            bool enabled = true,
            bool block = true,
            Uri? reportUri = null
        )
        {
            app.UseXXSSProtection(new XXssProtectionOptions
            {
                Enabled = enabled,
                Block = block,
                ReportUri = reportUri,
            });
        }


        internal sealed class XXSSProtectionMiddleware : IMiddleware
        {
            public XXSSProtectionMiddleware(XXssProtectionOptions options)
            {
                Options = options ?? throw new ArgumentNullException(nameof(options));
                _headerValue = Options.ToString();
            }

            private readonly StringValues _headerValue;

            public XXssProtectionOptions Options { get; }
            
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.Headers.TryGetValue(HeaderNames.ContentType, out var values) &&
                        values.Any(v => v.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)))
                    {
                        response.Headers["X-XSS-Protection"] = _headerValue;
                    }

                    return Task.CompletedTask;
                });

                await next.Invoke(context);
            }
        }
    }
}
