using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the Expect-CT header which allows sites to opt in to reporting and/or enforcement of Certificate Transparency requirements.
    /// </summary>
    /// <param name="app"></param>
    public static void UseExpectCT(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExpectCTMiddleware>();
    }

    public static IServiceCollection AddExpectCT(this IServiceCollection services, ExpectCTOptions options)
    {
        services.AddSingleton<ExpectCTMiddleware>();
        services.AddSingleton(options);
        return services;
    }

    /// <summary>
    /// Adds the Expect-CT header which allows sites to opt in to reporting and/or enforcement of Certificate Transparency requirements.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="enforce">Enforce Certificate Transparency.</param>
    /// <param name="maxAge">Specifies the number of seconds that the browser should cache and apply the received policy.</param>
    /// <param name="reportUri">Sets the URI the browser is going to report violations to.</param>
    public static IServiceCollection AddExpectCT(
        this IServiceCollection services,
        bool enforce = true,
        TimeSpan? maxAge = null,
        Uri? reportUri = null
    )
    {
        return services.AddExpectCT(new ExpectCTOptions
        {
            Enforce = enforce,
            MaxAge = maxAge,
            ReportUri = reportUri,
        });
    }


    internal sealed class ExpectCTMiddleware : IMiddleware
    {
        public ExpectCTMiddleware(ExpectCTOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _headerValue = Options.ToString();
        }

        private readonly StringValues _headerValue;

        public ExpectCTOptions Options { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;
                response.Headers["Expect-CT"] = _headerValue;
                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
