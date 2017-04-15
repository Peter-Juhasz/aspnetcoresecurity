using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds Content-Security-Policy header to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseContentSecurityPolicy(this IApplicationBuilder app, CspOptions options)
        {
            app.UseMiddleware<ContentSecurityPolicyMiddleware>(options);
        }


        internal sealed class ContentSecurityPolicyMiddleware
        {
            public ContentSecurityPolicyMiddleware(RequestDelegate next, IHostingEnvironment environment, CspOptions options)
            {
                _next = next;
                _environment = environment;
                Options = options ?? throw new ArgumentNullException(nameof(options));
            }

            private readonly RequestDelegate _next;
            private readonly IHostingEnvironment _environment;
            public CspOptions Options { get; }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType?.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        string csp = Options.ToString();

                        // allow inline styles and scripts for developer exception page
                        if (_environment.IsDevelopment())
                        {
                            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
                            {
                                csp = csp
                                    .Replace("style-src", "style-src 'unsafe-inline'")
                                    .Replace("script-src", "script-src 'unsafe-inline'")
                                ;
                            }
                        }

                        response.Headers["Content-Security-Policy"] = csp;
                        response.Headers["X-Content-Security-Policy"] = csp;
                        response.Headers["X-WebKit-CSP"] = csp;
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
