using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Prevents redirects to unsafe locations. Allows only the ones listed explicitly.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="allowedBaseUris">Allowed redirect locations.</param>
        public static void UseRedirectPolicy(this IApplicationBuilder app, IReadOnlyCollection<Uri> allowedBaseUris)
        {
            app.UseMiddleware<RedirectFilterMiddleware>(allowedBaseUris);
        }

        /// <summary>
        /// Prevents redirects to unsafe locations. Allows only the ones listed explicitly.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="allowedBaseUris">Allowed redirect locations.</param>
        public static void UseRedirectPolicy(this IApplicationBuilder app, params Uri[] allowedBaseUris)
        {
            app.UseRedirectPolicy(allowedBaseUris as IReadOnlyCollection<Uri>);
        }

        /// <summary>
        /// Prevents redirects to unsafe locations. Allows only the ones listed explicitly.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="allowedBaseUris">Allowed redirect locations.</param>
        public static void UseRedirectPolicy(this IApplicationBuilder app, params string[] allowedBaseUris)
        {
            app.UseRedirectPolicy(
                allowedBaseUris
                    .Select(s => new Uri(s, UriKind.Absolute))
                    .ToList()
            );
        }

        /// <summary>
        /// Prevents redirects to unsafe locations.
        /// </summary>
        /// <param name="app"></param>
        public static void UseRedirectPolicy(this IApplicationBuilder app)
        {
            app.UseRedirectPolicy(Array.Empty<Uri>());
        }


        internal sealed class RedirectFilterMiddleware
        {
            public RedirectFilterMiddleware(RequestDelegate next, IReadOnlyCollection<Uri> allowedBaseUris)
            {
                _next = next;
                AllowedBaseUris = allowedBaseUris ?? throw new ArgumentNullException(nameof(allowedBaseUris));
            }

            private readonly RequestDelegate _next;

            public IReadOnlyCollection<Uri> AllowedBaseUris { get; }

            internal bool IsAllowed(HttpRequest request, Uri location)
            {
                // normalize schema, if relative to request
                if (location.OriginalString.StartsWith("//"))
                    location = new Uri($"{request.Scheme}:{location}", UriKind.Absolute);

                // check absolute URIs only
                if (!location.IsAbsoluteUri)
                    return true;

                // check absolute URIs
                Uri requestUri = new Uri(request.GetDisplayUrl(), UriKind.Absolute);

                bool isSameHostAndPortOrUpgrade =
                    location.Host == requestUri.Host &&
                    (location.Port == requestUri.Port || (requestUri.Port == 80 && location.Port == 443));

                bool isAllowedLocation = AllowedBaseUris.Any(u => u.IsBaseOf(location));

                return isSameHostAndPortOrUpgrade || isAllowedLocation;
            }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    var request = context.Request;
                    var response = context.Response;

                    var location = response.GetTypedHeaders().Location;
                    if (location != null)
                    {
                        if (!IsAllowed(request, location))
                            throw new InvalidOperationException($"A potentially dangerous redirect was prevented to '{location}'.");
                    }

                    return Task.CompletedTask;
                });

                await _next(context);
            }
        }
    }
}
