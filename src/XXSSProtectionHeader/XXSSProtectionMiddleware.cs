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
        /// Adds the X-XSS-Protection header to each response with text/html media type.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseXXSSProtectionHeader(this IApplicationBuilder app, XXssProtectionOptions options)
        {
            app.UseMiddleware<XXSSProtectionMiddleware>(options);
        }

        public static void UseXXSSProtectionHeader(
            this IApplicationBuilder app,
            bool enabled = true,
            bool block = true,
            Uri reportUri = null
        )
        {
            app.UseXXSSProtectionHeader(new XXssProtectionOptions
            {
                Enabled = enabled,
                Block = block,
                ReportUri = reportUri,
            });
        }


        internal sealed class XXSSProtectionMiddleware
        {
            public XXSSProtectionMiddleware(RequestDelegate next, XXssProtectionOptions options)
            {
                _next = next;
                _headerValue = ConstructHeaderValue(options);
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;

            internal static string ConstructHeaderValue(XXssProtectionOptions options)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(options.Enabled ? "1" : "0");

                if (options.Block)
                    builder.Append("; mode=block");

                if (options.ReportUri != null)
                    builder.Append($"report={options.ReportUri}");

                return builder.ToString();
            }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType?.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        response.Headers["X-XSS-Protection"] = _headerValue;
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
