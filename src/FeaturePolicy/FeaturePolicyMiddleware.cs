using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Feature-Policy header to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseFeaturePolicy(this IApplicationBuilder app, FeatureDirectiveList options)
        {
            app.UseMiddleware<FeaturePolicyMiddleware>(options);
        }


        internal sealed class FeaturePolicyMiddleware
        {
            public FeaturePolicyMiddleware(RequestDelegate next, FeatureDirectiveList options)
            {
                _next = next;
                Options = options ?? throw new ArgumentNullException(nameof(options));
            }

            private readonly RequestDelegate _next;
            public FeatureDirectiveList Options { get; }
            
            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        response.Headers["Feature-Policy"] = Options.ToString();
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
