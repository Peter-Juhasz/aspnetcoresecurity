using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Prevents redirects to unsafe locations. Allows only the ones listed explicitly.
    /// </summary>
    /// <param name="app"></param>
    public static void UseRedirectPolicy(this IApplicationBuilder app)
    {
        app.UseMiddleware<RedirectFilterMiddleware>();
    }

    public static IServiceCollection AddRedirectPolicy(this IServiceCollection services, RedirectPolicyOptions options)
    {
        services.AddSingleton<RedirectFilterMiddleware>();
        services.AddSingleton(options);
        return services;
    }

    public static IServiceCollection AddRedirectPolicy(this IServiceCollection services, IReadOnlyCollection<Uri> allowedUris)
    {
        return services.AddRedirectPolicy(new RedirectPolicyOptions(allowedUris));
    }


    /// <summary>
    /// Prevents redirects to unsafe locations. Allows only the ones listed explicitly.
    /// </summary>
    /// <param name="allowedBaseUris">Allowed redirect locations.</param>
    public static IServiceCollection AddRedirectPolicy(this IServiceCollection services, params Uri[] allowedBaseUris)
    {
        return services.AddRedirectPolicy(allowedBaseUris as IReadOnlyCollection<Uri>);
    }

    /// <summary>
    /// Prevents redirects to unsafe locations. Allows only the ones listed explicitly.
    /// </summary>
    /// <param name="allowedBaseUris">Allowed redirect locations.</param>
    public static IServiceCollection AddRedirectPolicy(this IServiceCollection services, params string[] allowedBaseUris)
    {
        return services.AddRedirectPolicy(
            allowedBaseUris
                .Select(s => new Uri(s, UriKind.Absolute))
                .ToList()
        );
    }

    /// <summary>
    /// Prevents redirects to unsafe locations.
    /// </summary>
    public static IServiceCollection AddRedirectPolicy(this IServiceCollection services)
    {
        return services.AddRedirectPolicy(Array.Empty<Uri>());
    }


    internal sealed class RedirectFilterMiddleware : IMiddleware
    {
        public RedirectFilterMiddleware(RedirectPolicyOptions options)
        {
            AllowedBaseUris = options.AllowedBaseUris ?? throw new ArgumentNullException(nameof(options));
        }

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

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                var request = context.Request;
                var response = context.Response;

                if (response.Headers.TryGetValue(HeaderNames.Location, out var values))
                {
                    foreach (var location in values)
                    {
                        if (!Uri.TryCreate(location, UriKind.RelativeOrAbsolute, out var uri) ||
                            !IsAllowed(request, uri))
                            throw new InvalidOperationException($"A potentially dangerous redirect was prevented to '{location}'.");
                    }
                }

                return Task.CompletedTask;
            });

            await next(context);
        }
    }
}
