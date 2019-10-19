using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Content-Security-Policy header to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="directives"></param>
        public static void UseContentSecurityPolicy(this IApplicationBuilder app, CspDirectiveList directives)
        {
            app.UseMiddleware<ContentSecurityPolicyMiddleware>(directives);
        }


        internal sealed class ContentSecurityPolicyMiddleware
        {
            public ContentSecurityPolicyMiddleware(RequestDelegate next, IWebHostEnvironment environment, CspDirectiveList directives)
            {
                _next = next;
                _environment = environment;
                Directives = directives ?? throw new ArgumentNullException(nameof(directives));
            }

            private readonly RequestDelegate _next;
            private readonly IWebHostEnvironment _environment;
            public CspDirectiveList Directives { get; }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        var options = Directives;

                        // allow inline styles and scripts for developer exception page
                        if (_environment.IsDevelopment())
                        {
                            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
                            {
                                var developerOptions = Directives.Clone();
                                developerOptions.StyleSrc = (developerOptions.StyleSrc ?? StyleCspDirective.Empty).AddUnsafeInline();
                                developerOptions.ScriptSrc = (developerOptions.ScriptSrc ?? ScriptCspDirective.Empty).AddUnsafeInline();
                                options = developerOptions;
                            }
                        }

                        string csp = options.ToString();

                        response.Headers["Content-Security-Policy"] = csp;
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
