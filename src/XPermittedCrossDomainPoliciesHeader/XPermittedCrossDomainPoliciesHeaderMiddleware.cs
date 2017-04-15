using Microsoft.AspNetCore.Http;
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
        public static void UseXPermittedCrossDomainPoliciesHeader(this IApplicationBuilder app, PermittedCrossDomainPolicy policy = PermittedCrossDomainPolicy.None)
        {
            app.UseMiddleware<XPermittedCrossDomainPoliciesMiddleware>(policy);
        }


        internal sealed class XPermittedCrossDomainPoliciesMiddleware
        {
            public XPermittedCrossDomainPoliciesMiddleware(RequestDelegate next, PermittedCrossDomainPolicy policy)
            {
                _next = next;
                Policy = policy;
                _headerValue = HeaderValues[Policy];
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;

            private static readonly IReadOnlyDictionary<PermittedCrossDomainPolicy, string> HeaderValues = new Dictionary<PermittedCrossDomainPolicy, string>
            {
                { PermittedCrossDomainPolicy.None, "none" },
                { PermittedCrossDomainPolicy.MasterOnly, "master-only" },
                { PermittedCrossDomainPolicy.ByContentType, "by-content-type" },
                { PermittedCrossDomainPolicy.ByFtpFileName, "by-ftp-filename" },
                { PermittedCrossDomainPolicy.All, "all" },
            };

            public PermittedCrossDomainPolicy Policy { get; }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;
                    response.Headers["X-Permitted-Cross-Domain-Policies"] = _headerValue;

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
