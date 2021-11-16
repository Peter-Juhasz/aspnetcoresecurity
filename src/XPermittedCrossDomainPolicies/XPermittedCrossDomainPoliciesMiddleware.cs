using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using PeterJuhasz.AspNetCore.Extensions.Security;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds X-Permitted-Cross-Domain-Policies header to all responses.
    /// </summary>
    /// <param name="app"></param>
    public static void UseXPermittedCrossDomainPolicies(this IApplicationBuilder app)
    {
        app.UseMiddleware<XPermittedCrossDomainPoliciesMiddleware>();
    }

    public static IServiceCollection AddXPermittedCrossDomainPolicies(this IServiceCollection services, XPermittedCrossDomainPoliciesOptions options)
    {
        services.AddSingleton<XPermittedCrossDomainPoliciesMiddleware>();
        services.AddSingleton(options);
        return services;
    }

    public static IServiceCollection AddXPermittedCrossDomainPolicies(this IServiceCollection services, PermittedCrossDomainPolicy policy = PermittedCrossDomainPolicy.None)
    {
        return services.AddXPermittedCrossDomainPolicies(new XPermittedCrossDomainPoliciesOptions(policy));
    }


    internal sealed class XPermittedCrossDomainPoliciesMiddleware : IMiddleware
    {
        public XPermittedCrossDomainPoliciesMiddleware(XPermittedCrossDomainPoliciesOptions options)
        {
            Policy = options.Policy;
            _headerValue = HeaderValues[Policy];
        }

        private readonly StringValues _headerValue;

        private static readonly IReadOnlyDictionary<PermittedCrossDomainPolicy, string> HeaderValues = new Dictionary<PermittedCrossDomainPolicy, string>
            {
                { PermittedCrossDomainPolicy.None, "none" },
                { PermittedCrossDomainPolicy.MasterOnly, "master-only" },
                { PermittedCrossDomainPolicy.ByContentType, "by-content-type" },
                { PermittedCrossDomainPolicy.ByFtpFileName, "by-ftp-filename" },
                { PermittedCrossDomainPolicy.All, "all" },
            };

        public PermittedCrossDomainPolicy Policy { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;
                response.Headers["X-Permitted-Cross-Domain-Policies"] = _headerValue;

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
