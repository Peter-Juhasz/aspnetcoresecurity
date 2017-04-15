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
        public static void UseFrameOptionsHeader(this IApplicationBuilder app, FrameOptionsOptions options)
        {
            app.UseMiddleware<FrameOptionsMiddleware>(options);
        }

        /// <summary>
        /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configure"></param>
        public static void UseFrameOptionsHeader(this IApplicationBuilder app, Action<FrameOptionsOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            FrameOptionsOptions options = new FrameOptionsOptions();
            configure(options);
            app.UseFrameOptionsHeader(options);
        }

        /// <summary>
        /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="policy"></param>
        public static void UseFrameOptionsHeader(this IApplicationBuilder app, FrameOptionsPolicy policy = FrameOptionsPolicy.Deny)
        {
            if (policy == FrameOptionsPolicy.AllowFrom)
                throw new ArgumentException("This overload can't be used to configure ALLOW-FROM policy.", nameof(policy));

            app.UseFrameOptionsHeader(new FrameOptionsOptions(policy));
        }

        /// <summary>
        /// Adds the Frame-Options and X-Frame-Options headers to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="allowFromUri"></param>
        public static void UseFrameOptionsHeader(this IApplicationBuilder app, Uri allowFromUri)
        {
            app.UseFrameOptionsHeader(new FrameOptionsOptions(allowFromUri));
        }


        internal sealed class FrameOptionsMiddleware
        {
            public FrameOptionsMiddleware(RequestDelegate next, FrameOptionsOptions options)
            {
                if (options == null)
                    throw new ArgumentNullException(nameof(options));

                _next = next;
                _headerValue = ConstructHeaderValue(options);
                Options = options;
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;

            public FrameOptionsOptions Options { get; }
            
            internal static string ConstructHeaderValue(FrameOptionsOptions options)
            {
                switch (options.Policy)
                {
                    case FrameOptionsPolicy.Deny:
                        return "DENY";
                    case FrameOptionsPolicy.SameOrigin:
                        return "SAMEORIGIN";
                    case FrameOptionsPolicy.AllowFrom:
                        return $"ALLOW-FROM {options.AllowFromUri}";

                    default: throw new NotSupportedException($"Not supported Frame-Options policy '{options.Policy}'.");
                }
            }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType?.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
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
