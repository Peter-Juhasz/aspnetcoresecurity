using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the Report-To header to each response.
    /// </summary>
    /// <param name="app"></param>
    public static void UseReportTo(this IApplicationBuilder app)
    {
        app.UseMiddleware<ReportToMiddleware>();
    }

    public static IServiceCollection AddReportTo(this IServiceCollection services, ReportToOptions options)
    {
        services.AddSingleton<ReportToMiddleware>();
        services.AddSingleton(options);
        return services;
    }

    public static IServiceCollection AddReportTo(this IServiceCollection services, IReadOnlyList<ReportingGroup> groups)
    {
        return services.AddReportTo(new ReportToOptions(groups));
    }

    /// <summary>
    /// Adds the Report-To header to each response.
    /// </summary>
    public static IServiceCollection AddReportTo(this IServiceCollection services, ReportingGroup group) => services.AddReportTo(new[] { group });


    internal sealed class ReportToMiddleware : IMiddleware
    {
        public ReportToMiddleware(ReportToOptions options)
        {
            Groups = options.Groups;
            _headerValue = Groups.Count switch
            {
                0 => throw new ArgumentException("At least one group must be included for the Report-To header.", nameof(options)),
                1 => Groups.Single().ToString(),
                _ => String.Join(',', Groups)
            };
        }

        private readonly StringValues _headerValue;

        public IReadOnlyList<ReportingGroup> Groups { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;
                response.Headers["Report-To"] = _headerValue;
                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
