using System;
using System.Collections.Generic;

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
    }
}
