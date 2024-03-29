﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// Adds the Permissions-Policy header to responses with content type text/html.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="options"></param>
    public static void UsePermissionsPolicy(this IApplicationBuilder app)
    {
        app.UseMiddleware<PermissionsPolicyMiddleware>();
    }

    public static IServiceCollection AddPermissionsPolicy(this IServiceCollection services, PermissionsPolicyDirectiveList options)
    {
        services.AddSingleton<PermissionsPolicyMiddleware>();
        services.AddSingleton(options);
        return services;
    }


    internal sealed class PermissionsPolicyMiddleware : IMiddleware
    {
        public PermissionsPolicyMiddleware(PermissionsPolicyDirectiveList options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _headerValue = Options.ToString();
        }

        private const string HeaderName = "Permissions-Policy";
        private readonly StringValues _headerValue;

        public PermissionsPolicyDirectiveList Options { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                HttpResponse response = context.Response;

                if (response.Headers.TryGetValue(HeaderNames.ContentType, out var values) &&
                    values.Any(v => v.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)))
                {
                    if (context.Items.TryGetValue(nameof(PermissionsPolicyDirectiveList), out var value))
                    {
                        var changes = (IList<Change>)value!;

                    }

                    response.Headers[HeaderName] = _headerValue;
                }

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
