using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Report-To header to each response.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="groups"></param>
        public static void UseReportTo(this IApplicationBuilder app, IReadOnlyList<ReportingGroup> groups)
        {
            app.UseMiddleware<ReportToMiddleware>(groups);
        }

        /// <summary>
        /// Adds the Report-To header to each response.
        /// </summary>
        public static void UseReportTo(this IApplicationBuilder app, ReportingGroup group) => app.UseReportTo(new[] { group });


        internal sealed class ReportToMiddleware : IMiddleware
        {
            public ReportToMiddleware(IReadOnlyList<ReportingGroup> groups)
            {
                Groups = groups;
                _headerValue = Groups.Count switch
                {
                    0 => throw new ArgumentException("At least one group must be included for the Report-To header.", nameof(groups)),
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
}
