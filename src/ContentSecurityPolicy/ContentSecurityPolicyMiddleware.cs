using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class AppBuilderExtensions
    {
        /// <summary>
        /// Adds Content-Security-Policy header to responses with content type text/html.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseContentSecurityPolicy(this IApplicationBuilder app, CspOptions options)
        {
            app.UseMiddleware<ContentSecurityPolicyMiddleware>(options);
        }


        internal sealed class ContentSecurityPolicyMiddleware
        {
            public ContentSecurityPolicyMiddleware(RequestDelegate next, IHostingEnvironment environment, CspOptions options)
            {
                _next = next;
                _environment = environment;
                Options = options ?? throw new ArgumentNullException(nameof(options));
            }

            private readonly RequestDelegate _next;
            private readonly IHostingEnvironment _environment;
            public CspOptions Options { get; }

            internal static string ConstructHeaderValue(CspOptions options)
            {
                ICollection<string> directives = new List<string>();

                if (options.DefaultSrc != null) directives.Add($"default-src {options.DefaultSrc}");
                if (options.BaseUri != null) directives.Add($"base-uri {options.BaseUri}");
                if (options.ChildSrc != null) directives.Add($"child-src {options.ChildSrc}");
                if (options.ConnectSrc != null) directives.Add($"connect-src {options.ConnectSrc}");
                if (options.FontSrc != null) directives.Add($"font-src {options.FontSrc}");
                if (options.FormAction != null) directives.Add($"font-src {options.FormAction}");
                if (options.FrameAncestors != null) directives.Add($"frame-ancestors {options.FrameAncestors}");
                if (options.FrameSrc != null) directives.Add($"frame-src {options.FrameSrc}");
                if (options.ImgSrc != null) directives.Add($"img-src {options.ImgSrc}");
                if (options.ManifestSrc != null) directives.Add($"manifest-src {options.ManifestSrc}");
                if (options.MediaSrc != null) directives.Add($"media-src {options.MediaSrc}");
                if (options.NavigationTo != null) directives.Add($"navigation-to {options.NavigationTo}");
                if (options.ObjectSrc != null) directives.Add($"object-src {options.ObjectSrc}");
                if (options.ScriptSrc != null) directives.Add($"script-src {options.ScriptSrc}");
                if (options.StyleSrc != null) directives.Add($"style-src {options.StyleSrc}");
                if (options.WorkerSrc != null) directives.Add($"worker-src {options.WorkerSrc}");

                if (options.BlockAllMixedContent) directives.Add("block-all-mixed-content");
                if (options.UpgradeInsecureRequests) directives.Add("upgrade-insecure-requests");
                if (options.ReflectedXss != null) directives.Add(ConstructHeaderValue(options.ReflectedXss.Value));
                if (options.PluginTypes?.Any() ?? false) directives.Add($"plugin-types {String.Join(" ", options.PluginTypes)}");
                if (options.RequireSriFor != null) directives.Add(ConstructHeaderValue(options.RequireSriFor.Value));
                if (options.Sandbox != null) directives.Add(ConstructHeaderValue(options.Sandbox.Value));

#pragma warning disable CS0612
                if (options.ReportUri != null) directives.Add($"report-uri {options.ReportUri}");
#pragma warning restore CS0612

                return String.Join("; ", directives);
            }

            internal static string ConstructHeaderValue(CspReflectedXss value)
            {
                return $"reflected-xss {value.ToString().ToLowerInvariant()}";
            }

            internal static string ConstructHeaderValue(CspRequireSRIResources values)
            {
                ICollection<string> tokens = new List<string>();

                if (values.HasFlag(CspRequireSRIResources.Style)) tokens.Add("style");
                if (values.HasFlag(CspRequireSRIResources.Script)) tokens.Add("script");

                return $"require-sri-for {String.Join(" ", tokens)}";
            }

            internal static string ConstructHeaderValue(CspSandboxRules values)
            {
                if (values == CspSandboxRules.Sandbox)
                    return "sandbox";

                ICollection<string> tokens = new List<string>();

                if (values.HasFlag(CspSandboxRules.AllowForms)) tokens.Add("allow-forms");
                if (values.HasFlag(CspSandboxRules.AllowModals)) tokens.Add("allow-modals");
                if (values.HasFlag(CspSandboxRules.AllowOrientationLock)) tokens.Add("allow-orientation-lock");
                if (values.HasFlag(CspSandboxRules.AllowPointerLock)) tokens.Add("allow-pointer-lock");
                if (values.HasFlag(CspSandboxRules.AllowPopups)) tokens.Add("allow-popups");
                if (values.HasFlag(CspSandboxRules.AllowPopupsToEscapeSandbox)) tokens.Add("allow-popups-to-escape-sandbox");
                if (values.HasFlag(CspSandboxRules.AllowPresentation)) tokens.Add("allow-presentation");
                if (values.HasFlag(CspSandboxRules.AllowSameOrigin)) tokens.Add("allow-same-origin");
                if (values.HasFlag(CspSandboxRules.AllowScripts)) tokens.Add("allow-scripts");
                if (values.HasFlag(CspSandboxRules.AllowTopNavigation)) tokens.Add("allow-top-navigation");

                return $"sandbox {String.Join(" ", tokens)}";
            }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(() =>
                {
                    HttpResponse response = context.Response;

                    if (response.GetTypedHeaders().ContentType?.MediaType?.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        string csp = ConstructHeaderValue(Options);

                        // allow inline styles and scripts for developer exception page
                        if (_environment.IsDevelopment())
                        {
                            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
                            {
                                csp = csp
                                    .Replace("style-src", "style-src 'unsafe-inline'")
                                    .Replace("script-src", "script-src 'unsafe-inline'")
                                ;
                            }
                        }

                        response.Headers["Content-Security-Policy"] = csp;
                        response.Headers["X-Content-Security-Policy"] = csp;
                        response.Headers["X-WebKit-CSP"] = csp;
                    }

                    return Task.CompletedTask;
                });

                await _next.Invoke(context);
            }
        }
    }
}
