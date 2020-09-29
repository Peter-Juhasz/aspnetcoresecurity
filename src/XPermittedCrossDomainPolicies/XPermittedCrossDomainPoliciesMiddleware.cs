using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds X-Permitted-Cross-Domain-Policies header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="policy"></param>
        public static void UseXPermittedCrossDomainPolicies(this IApplicationBuilder app, PermittedCrossDomainPolicy policy = PermittedCrossDomainPolicy.None)
        {
            app.UseMiddleware<XPermittedCrossDomainPoliciesMiddleware>(policy);
        }


        internal sealed class XPermittedCrossDomainPoliciesMiddleware : IMiddleware
        {
            public XPermittedCrossDomainPoliciesMiddleware(PermittedCrossDomainPolicy policy)
            {
                Policy = policy;
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
}
