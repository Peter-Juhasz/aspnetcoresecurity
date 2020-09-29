using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Permissions-Policy header to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UsePermissionsPolicy(this IApplicationBuilder app, PermissionsPolicyDirectiveList options)
        {
            app.UseMiddleware<PermissionsPolicyMiddleware>(options);
        }


        internal sealed class PermissionsPolicyMiddleware
        {
            public PermissionsPolicyMiddleware(RequestDelegate next, PermissionsPolicyDirectiveList options)
            {
                _next = next;
                Options = options ?? throw new ArgumentNullException(nameof(options));
            }

            private const string HeaderName = "Permissions-Policy";

            private readonly RequestDelegate _next;
            public PermissionsPolicyDirectiveList Options { get; }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.Headers.TryGetValue(HeaderNames.ContentType, out var values) &&
                        values.Any(v => v.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)))
                    {
                        response.Headers[HeaderName] = Options.ToString();
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
