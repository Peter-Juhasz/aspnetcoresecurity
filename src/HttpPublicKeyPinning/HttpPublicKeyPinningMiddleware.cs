﻿using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the Public-Key-Pinning header to all responses.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configure"></param>
        public static void UseHttpPublicKeyPinning(this IApplicationBuilder app, Action<HttpPublicKeyPinningOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var options = new HttpPublicKeyPinningOptions();
            configure(options);
            app.UseMiddleware<HttpPublicKeyPinningMiddleware>(options);
        }


        internal sealed class HttpPublicKeyPinningMiddleware
        {
            public HttpPublicKeyPinningMiddleware(RequestDelegate next, HttpPublicKeyPinningOptions options)
            {
                if (options == null)
                    throw new ArgumentNullException(nameof(options));

                if (!options.Pins.Any())
                    throw new InvalidOperationException("At least one fingerprint have to be pinned.");

                _next = next;
                Options = options;
                _headerValue = ConstructHeaderValue(Options);
            }


            private readonly RequestDelegate _next;
            private readonly string _headerValue;

            public HttpPublicKeyPinningOptions Options { get; }

            internal static string ConstructHeaderValue(HttpPublicKeyPinningOptions options)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in options.Pins)
                {
                    if (builder.Length > 0)
                        builder.Append("; ");

                    builder.Append($"pin-{item.Algorithm.ToString().ToLowerInvariant()}=\"{item.Base64Fingerprint}\"");
                }

                builder.Append($"; max-age={options.MaxAge.TotalSeconds:F0}");

                if (options.IncludeSubDomains)
                    builder.Append("; includeSubDomains");

                if (options.ReportUri != null)
                    builder.Append($"; report-uri=\"{options.ReportUri}\"");

                return builder.ToString();
            }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;
                    response.Headers["Strict-Transport-Security"] = _headerValue;

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}