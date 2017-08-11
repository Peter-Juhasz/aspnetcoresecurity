using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Strict-Transport-Security header to all secure responses with the default settings (max-age: 1 year).
        /// </summary>
        /// <param name="app"></param>
        public static void UseHttpStrictTransportSecurity(this IApplicationBuilder app)
        {
            app.UseHttpStrictTransportSecurity(new HttpStrictTransportSecurityOptions
            {
                MaxAge = TimeSpan.FromDays(365),
                IncludeSubDomains = true,
                Preload = true,
            });
        }

        /// <summary>
        /// Adds the Strict-Transport-Security header to all secure responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseHttpStrictTransportSecurity(this IApplicationBuilder app, HttpStrictTransportSecurityOptions options)
        {
            app.UseMiddleware<HttpStrictTransportSecurityMiddleware>(options);
        }


        internal sealed class HttpStrictTransportSecurityMiddleware
        {
            public HttpStrictTransportSecurityMiddleware(RequestDelegate next, HttpStrictTransportSecurityOptions options)
            {
                _next = next;
                Options = options ?? throw new ArgumentNullException(nameof(options));
                _headerValue = Options.ToString();
            }


            private readonly RequestDelegate _next;
            private readonly string _headerValue;
            
            public HttpStrictTransportSecurityOptions Options { get; }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Request.IsHttps)
                    {
                        HttpResponse response = context.Response;
                        response.Headers["Strict-Transport-Security"] = _headerValue;
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
