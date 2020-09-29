using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Referrer-Policy header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="policy"></param>
        public static void UseReferrerPolicy(this IApplicationBuilder app, ReferrerPolicy policy = ReferrerPolicy.SameOrigin)
        {
            app.UseMiddleware<ReferrerPolicyMiddleware>(policy);
        }


        internal sealed class ReferrerPolicyMiddleware : IMiddleware
        {
            public ReferrerPolicyMiddleware(ReferrerPolicy policy)
            {
                Policy = policy;
                _headerValue = HeaderValues[Policy];
            }

            private readonly StringValues _headerValue;

            public ReferrerPolicy Policy { get; }

            private static readonly IReadOnlyDictionary<ReferrerPolicy, string> HeaderValues = new Dictionary<ReferrerPolicy, string>
            {
                { ReferrerPolicy.NoReferrer, "no-referrer" },
                { ReferrerPolicy.NoReferrerWhenDowngrade, "no-referrer-when-downgrade" },
                { ReferrerPolicy.SameOrigin, "same-origin" },
                { ReferrerPolicy.Origin, "origin" },
                { ReferrerPolicy.StrictOrigin, "strict-origin" },
                { ReferrerPolicy.OriginWhenCrossOrigin, "origin-when-cross-origin" },
                { ReferrerPolicy.StrictOriginWhenCrossOrigin, "strict-origin-when-cross-origin" },
                { ReferrerPolicy.UnsafeUrl, "unsafe-url" },
            };

            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                context.Response.OnStarting(() =>
                {
                    var response = context.Response;
                    response.Headers["Referrer-Policy"] = _headerValue;

                    return Task.CompletedTask;
                });

                await next(context);
            }
        }
    }
}
