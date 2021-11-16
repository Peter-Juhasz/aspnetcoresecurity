using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the Public-Key-Pinning header to all responses.
    /// </summary>
    /// <param name="app"></param>
    [Obsolete]
    public static void UseHttpPublicKeyPinning(this IApplicationBuilder app)
    {
        app.UseMiddleware<HttpPublicKeyPinningMiddleware>();
    }

    [Obsolete]
    public static IServiceCollection AddHttpPublicKeyPinning(this IServiceCollection services, Action<HttpPublicKeyPinningOptions> configure)
    {
        services.AddSingleton<HttpPublicKeyPinningMiddleware>();
        var options = new HttpPublicKeyPinningOptions();
        configure(options);
        services.AddSingleton(options);
        return services;
    }


    internal sealed class HttpPublicKeyPinningMiddleware : IMiddleware
    {
        public HttpPublicKeyPinningMiddleware(HttpPublicKeyPinningOptions options)
        {
            if (!options.Pins.Any())
                throw new InvalidOperationException("At least one fingerprint have to be pinned.");

            Options = options;
            _headerValue = Options.ToString();
        }

        private readonly StringValues _headerValue;

        public HttpPublicKeyPinningOptions Options { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;
                response.Headers["Public-Key-Pins"] = _headerValue;

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
