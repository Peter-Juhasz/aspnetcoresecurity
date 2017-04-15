using System;
using System.Collections.Generic;
using System.Linq;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class CspOptions
    {
        /// <summary>
        /// The base-uri directive restricts the URLs which can be used in a document's &lt;base&gt; element. If this value is absent, then any URI is allowed. If this directive is absent, the user agent will use the value in the &lt;base&gt; element.
        /// </summary>
        public CspDirective BaseUri { get; set; }

        public CspDirective DefaultSrc { get; set; } = CspDirective.None;

        public StyleCspDirective StyleSrc { get; set; }

        public ScriptCspDirective ScriptSrc { get; set; }

        public CspDirective ImgSrc { get; set; }

        public CspDirective FontSrc { get; set; }

        public CspDirective ConnectSrc { get; set; }

        public CspDirective ObjectSrc { get; set; }

        public CspDirective MediaSrc { get; set; }

        public CspDirective ManifestSrc { get; set; }

        public CspDirective FrameSrc { get; set; }

        public CspDirective ChildSrc { get; set; }

        public CspDirective FormAction { get; set; }

        public CspDirective FrameAncestors { get; set; }

        public CspDirective WorkerSrc { get; set; }

        public CspDirective NavigationTo { get; set; }

        public CspSandboxRules? Sandbox { get; set; }

        public IReadOnlyCollection<string> PluginTypes { get; set; }

        public CspRequireSRIResources? RequireSriFor { get; set; }

        public CspReflectedXss? ReflectedXss { get; set; } = CspReflectedXss.Block;

        /// <summary>
        /// The upgrade-insecure-requests directive instructs user agents to treat all of a site's insecure URLs (those served over HTTP) as though they have been replaced with secure URLs (those served over HTTPS).
        /// </summary>
        public bool UpgradeInsecureRequests { get; set; } = true;

        /// <summary>
        /// The block-all-mixed-content directive prevents loading any assets using HTTP when the page is loaded using HTTPS.
        /// </summary>
        public bool BlockAllMixedContent { get; set; } = true;

        [Obsolete]
        public Uri ReportUri { get; set; }


        public override string ToString()
        {
            ICollection<string> directives = new List<string>();

            if (this.DefaultSrc != null) directives.Add($"default-src {this.DefaultSrc}");
            if (this.BaseUri != null) directives.Add($"base-uri {this.BaseUri}");
            if (this.ChildSrc != null) directives.Add($"child-src {this.ChildSrc}");
            if (this.ConnectSrc != null) directives.Add($"connect-src {this.ConnectSrc}");
            if (this.FontSrc != null) directives.Add($"font-src {this.FontSrc}");
            if (this.FormAction != null) directives.Add($"font-src {this.FormAction}");
            if (this.FrameAncestors != null) directives.Add($"frame-ancestors {this.FrameAncestors}");
            if (this.FrameSrc != null) directives.Add($"frame-src {this.FrameSrc}");
            if (this.ImgSrc != null) directives.Add($"img-src {this.ImgSrc}");
            if (this.ManifestSrc != null) directives.Add($"manifest-src {this.ManifestSrc}");
            if (this.MediaSrc != null) directives.Add($"media-src {this.MediaSrc}");
            if (this.NavigationTo != null) directives.Add($"navigation-to {this.NavigationTo}");
            if (this.ObjectSrc != null) directives.Add($"object-src {this.ObjectSrc}");
            if (this.ScriptSrc != null) directives.Add($"script-src {this.ScriptSrc}");
            if (this.StyleSrc != null) directives.Add($"style-src {this.StyleSrc}");
            if (this.WorkerSrc != null) directives.Add($"worker-src {this.WorkerSrc}");

            if (this.BlockAllMixedContent) directives.Add("block-all-mixed-content");
            if (this.UpgradeInsecureRequests) directives.Add("upgrade-insecure-requests");
            if (this.ReflectedXss != null) directives.Add(ConstructHeaderValue(this.ReflectedXss.Value));
            if (this.PluginTypes?.Any() ?? false) directives.Add($"plugin-types {String.Join(" ", this.PluginTypes)}");
            if (this.RequireSriFor != null) directives.Add(ConstructHeaderValue(this.RequireSriFor.Value));
            if (this.Sandbox != null) directives.Add(ConstructHeaderValue(this.Sandbox.Value));

#pragma warning disable CS0612
            if (this.ReportUri != null) directives.Add($"report-uri {this.ReportUri}");
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
    }
}
