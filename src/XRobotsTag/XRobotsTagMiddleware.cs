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

            app.UseXRobotsTag(new RobotsTagDirectiveList
            {
                Directives = directives,
            });
        }

        /// <summary>
        /// Adds the X-Robots-Tag header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="directives"></param>
        public static void UseXRobotsTag(this IApplicationBuilder app, RobotsTagDirectiveList directives)
        {
            app.UseMiddleware<XRobotsTagMiddleware>(directives);
        }


        internal sealed class XRobotsTagMiddleware
        {
            public XRobotsTagMiddleware(RequestDelegate next, RobotsTagDirectiveList headerValue)
            {
                _next = next;
                Directives = headerValue ?? throw new ArgumentNullException(nameof(headerValue));
                _headerValue = Directives.ToString();
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;
            
            public RobotsTagDirectiveList Directives { get; }

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
