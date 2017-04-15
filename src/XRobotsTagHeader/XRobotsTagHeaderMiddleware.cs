using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void UseXRobotsTagHeader(this IApplicationBuilder app, bool noIndex = true, bool noFollow = true)
        {
            RobotsTagDirectives directives = noIndex
                ? noFollow
                    ? RobotsTagDirectives.NoIndex | RobotsTagDirectives.NoFollow
                    : RobotsTagDirectives.NoIndex
                : noFollow
                    ? RobotsTagDirectives.NoFollow
                    : RobotsTagDirectives.All
            ;

            app.UseXRobotsTagHeader(new XRobotsTagHeaderValue
            {
                Directives = directives,
            });
        }

        /// <summary>
        /// Adds the X-Robots-Tag header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="headerValue"></param>
        public static void UseXRobotsTagHeader(this IApplicationBuilder app, XRobotsTagHeaderValue headerValue)
        {
            app.UseMiddleware<XRobotsTagMiddleware>(headerValue);
        }


        internal sealed class XRobotsTagMiddleware
        {
            public XRobotsTagMiddleware(RequestDelegate next, XRobotsTagHeaderValue headerValue)
            {
                _next = next;
                Options = headerValue ?? throw new ArgumentNullException(nameof(headerValue));
                _headerValue = ConstructHeaderValue(Options);
            }


            private readonly RequestDelegate _next;
            private readonly string _headerValue;
            
            public XRobotsTagHeaderValue Options { get; }

            public static string ConstructHeaderValue(XRobotsTagHeaderValue options)
            {
                ICollection<string> directives = new HashSet<string>();

                if (!options.Directives.HasValue ||
                    options.Directives.Value == default(RobotsTagDirectives) ||
                    options.Directives.Value.HasFlag(RobotsTagDirectives.All))
                {
                    directives.Add("all");
                }
                else
                { 
                    foreach (var directive in Enum.GetValues(typeof(RobotsTagDirectives)).Cast<RobotsTagDirectives>()
                        .Where(d => options.Directives.Value.HasFlag(d))
                    )
                        directives.Add(directive.ToString().ToLowerInvariant());
                }

                if (options.UnavailableAfter != null)
                    directives.Add($"unavailable_after: {options.UnavailableAfter.Value.UtcDateTime:dddd, d-MMM-yyyy H:mm:ss UTC}");

                return String.Join(", ", directives);
            }

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
