using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Expect-CT header which allows sites to opt in to reporting and/or enforcement of Certificate Transparency requirements.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseExpectCT(this IApplicationBuilder app, ExpectCTOptions options)
        {
            app.UseMiddleware<ExpectCTMiddleware>(options);
        }

        /// <summary>
        /// Adds the Expect-CT header which allows sites to opt in to reporting and/or enforcement of Certificate Transparency requirements.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="enforce">Enforce Certificate Transparency.</param>
        /// <param name="maxAge">Specifies the number of seconds that the browser should cache and apply the received policy.</param>
        /// <param name="reportUri">Sets the URI the browser is going to report violations to.</param>
        public static void UseExpectCT(
            this IApplicationBuilder app,
            bool enforce = true,
            TimeSpan? maxAge = null,
            Uri? reportUri = null
        )
        {
            app.UseExpectCT(new ExpectCTOptions
            {
                Enforce = enforce,
                MaxAge = maxAge,
                ReportUri = reportUri,
            });
        }


        internal sealed class ExpectCTMiddleware
        {
            public ExpectCTMiddleware(RequestDelegate next, ExpectCTOptions options)
            {
                _next = next;
                Options = options ?? throw new ArgumentNullException(nameof(options));
                _headerValue = Options.ToString();
            }

            private readonly RequestDelegate _next;
            private readonly string _headerValue;

            public ExpectCTOptions Options { get; }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;
                    response.Headers["Expect-CT"] = _headerValue;
                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
