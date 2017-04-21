using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the X-Robots-Tag header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="noIndex"></param>
        /// <param name="noFollow"></param>
        public static void UseXRobotsTag(this IApplicationBuilder app, bool noIndex = true, bool noFollow = true)
        {
            RobotsTagDirectives directives = noIndex
                ? noFollow
                    ? RobotsTagDirectives.NoIndex | RobotsTagDirectives.NoFollow
                    : RobotsTagDirectives.NoIndex
                : noFollow
                    ? RobotsTagDirectives.NoFollow
                    : RobotsTagDirectives.All
            ;

            app.UseXRobotsTag(new XRobotsTagHeaderValue
            {
                Directives = directives,
            });
        }

        /// <summary>
        /// Adds the X-Robots-Tag header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="headerValue"></param>
        public static void UseXRobotsTag(this IApplicationBuilder app, XRobotsTagHeaderValue headerValue)
        {
            app.UseMiddleware<XRobotsTagMiddleware>(headerValue);
        }


        internal sealed class XRobotsTagMiddleware
        {
            public XRobotsTagMiddleware(RequestDelegate next, XRobotsTagHeaderValue headerValue)
            {
                _next = next;
                Options = headerValue ?? throw new ArgumentNullException(nameof(headerValue));
                _headerValue = Options.ToString();
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;
            
            public XRobotsTagHeaderValue Options { get; }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;
                    response.Headers["X-Robots-Tag"] = _headerValue;

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
