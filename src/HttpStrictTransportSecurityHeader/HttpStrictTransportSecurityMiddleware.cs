using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Strict-Transport-Security header to all responses with the default settings (max-age: 1 year).
        /// </summary>
        /// <param name="app"></param>
        public static void UseHttpStrictTransportSecurityHeader(this IApplicationBuilder app)
        {
            app.UseHttpStrictTransportSecurityHeader(new HttpStrictTransportSecurityOptions
            {
                MaxAge = TimeSpan.FromDays(365),
                IncludeSubDomains = true,
                Preload = true,
            });
        }

        /// <summary>
        /// Adds the Strict-Transport-Security header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseHttpStrictTransportSecurityHeader(this IApplicationBuilder app, HttpStrictTransportSecurityOptions options)
        {
            app.UseMiddleware<HttpStrictTransportSecurityMiddleware>(options);
        }


        internal sealed class HttpStrictTransportSecurityMiddleware
        {
            public HttpStrictTransportSecurityMiddleware(RequestDelegate next, HttpStrictTransportSecurityOptions options)
            {
                if (options == null)
                    throw new ArgumentNullException(nameof(options));

                _next = next;
                _headerValue = ConstructHeaderValue(options);
                Options = options;
            }


            private readonly RequestDelegate _next;
            private readonly string _headerValue;
            
            public HttpStrictTransportSecurityOptions Options { get; }

            private static string ConstructHeaderValue(HttpStrictTransportSecurityOptions options)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"max-age={options.MaxAge.TotalSeconds:F0}");

                if (options.IncludeSubDomains)
                    builder.Append("; includeSubDomains");

                if (options.Preload)
                    builder.Append("; preload");

                return builder.ToString();
            }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;
                    response.Headers["Strict-Transport-Security"] = _headerValue;

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
