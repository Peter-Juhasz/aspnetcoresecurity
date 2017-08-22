using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseFrameOptions(this IApplicationBuilder app, FrameOptionsOptions options)
        {
            app.UseMiddleware<FrameOptionsMiddleware>(options);
        }

        /// <summary>
        /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configure"></param>
        public static void UseFrameOptions(this IApplicationBuilder app, Action<FrameOptionsOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            FrameOptionsOptions options = new FrameOptionsOptions();
            configure(options);
            app.UseFrameOptions(options);
        }

        /// <summary>
        /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="policy"></param>
        public static void UseFrameOptions(this IApplicationBuilder app, FrameOptionsPolicy policy = FrameOptionsPolicy.Deny)
        {
            if (policy == FrameOptionsPolicy.AllowFrom)
                throw new ArgumentException("This overload can't be used to configure ALLOW-FROM policy.", nameof(policy));

            app.UseFrameOptions(new FrameOptionsOptions(policy));
        }

        /// <summary>
        /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="allowFromUri"></param>
        public static void UseFrameOptions(this IApplicationBuilder app, Uri allowFromUri)
        {
            app.UseFrameOptions(new FrameOptionsOptions(allowFromUri));
        }


        internal sealed class FrameOptionsMiddleware
        {
            public FrameOptionsMiddleware(RequestDelegate next, FrameOptionsOptions options)
            {
                _next = next;
                Options = options ?? throw new ArgumentNullException(nameof(options));
                _headerValue = Options.ToString();
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;

            public FrameOptionsOptions Options { get; }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        response.Headers["X-Frame-Options"] = _headerValue;
                        response.Headers["Frame-Options"] = _headerValue;
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
