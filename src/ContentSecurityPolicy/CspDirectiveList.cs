using System;
using System.Collections.Generic;
using System.Linq;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents the value of the Content-Security-Policy header.
/// </summary>
/// <param name="DefaultSrc">Serves as a fallback for the other fetch directives.</param>
/// <param name="BaseUri">Restricts the URLs which can be used in a document's &lt;base&gt; element.</param>
/// <param name="StyleSrc">Specifies valid sources for stylesheets.</param>
/// <param name="StyleSrcAttr">Specifies valid sources for inline styles applied to individual DOM elements.</param>
/// <param name="StyleSrcElem">Specifies valid sources for stylesheets &lt;style&gt; elements and &lt;link&gt; elements with rel=&quot;stylesheet&quot;.</param>
/// <param name="ScriptSrc">Specifies valid sources for JavaScript.</param>
/// <param name="ScriptSrcAttr">Specifies valid sources for JavaScript inline event handlers.</param>
/// <param name="ScriptSrcElem">Specifies valid sources for JavaScript &lt;script&gt; elements.</param>
/// <param name="ImgSrc">Specifies valid sources of images and favicons.</param>
/// <param name="FontSrc">Specifies valid sources for fonts loaded using @font-face.</param>
/// <param name="ConnectSrc">Restricts the URLs which can be loaded using script interfaces.</param>
/// <param name="ObjectSrc">Specifies valid sources for the &lt;object&gt;, &lt;embed&gt;, and &lt;applet&gt; elements.</param>
/// <param name="PrefetchSrc">Specifies valid resources that may be prefetched or prerendered.</param>
/// <param name="MediaSrc">Specifies valid sources for loading media using the &lt;audio&gt; and &lt;video&gt; elements.</param>
/// <param name="ManifestSrc">Specifies valid sources of application manifest files.</param>
/// <param name="FrameSrc">Specifies valid sources for nested browsing contexts loading using elements such as &lt;frame&gt; and &lt;iframe&gt;.</param>
/// <param name="ChildSrc">Defines the valid sources for web workers and nested browsing contexts loaded using elements such as &lt;frame&gt; and &lt;iframe&gt;.</param>
/// <param name="FormAction">Restricts the URLs which can be used as the target of a form submissions from a given context.</param>
/// <param name="FrameAncestors">Specifies valid parents that may embed a page using &lt;frame&gt;, &lt;iframe&gt;, &lt;object&gt;, &lt;embed&gt;, or &lt;applet&gt;.</param>
/// <param name="WorkerSrc">Specifies valid sources for Worker, SharedWorker, or ServiceWorker scripts.</param>
/// <param name="NavigationTo">Restricts the URLs to which a document can navigate by any means (a, form, window.location, window.open, etc.).</param>
/// <param name="Sandbox">Enables a sandbox for the requested resource similar to the &lt;iframe&gt; sandbox attribute.</param>
/// <param name="PluginTypes">Restricts the set of plugins that can be embedded into a document by limiting the types of resources which can be loaded.</param>
/// <param name="RequireSriFor">Requires the use of SRI for scripts or styles on the page.</param>
/// <param name="UpgradeInsecureRequests">Instructs user agents to treat all of a site's insecure URLs (those served over HTTP) as though they have been replaced with secure URLs (those served over HTTPS).</param>
/// <param name="BlockAllMixedContent">Prevents loading any assets using HTTP when the page is loaded using HTTPS.</param>
/// <param name="ReportTo">The name of the Report-To group for reporting violations.</param>
/// <param name="ReportUri">The URI the browser is going to report violations to.</param>
public record class CspDirectiveList(
    CspDirective DefaultSrc,
    CspDirective? BaseUri = null,
    StyleCspDirective? StyleSrc = null,
    StyleCspDirective? StyleSrcAttr = null,
    StyleCspDirective? StyleSrcElem = null,
    ScriptCspDirective? ScriptSrc = null,
    ScriptCspDirective? ScriptSrcAttr = null,
    ScriptCspDirective? ScriptSrcElem = null,
    CspDirective? ImgSrc = null,
    CspDirective? FontSrc = null,
    CspDirective? ConnectSrc = null,
    CspDirective? ObjectSrc = null,
    CspDirective? PrefetchSrc = null,
    CspDirective? MediaSrc = null,
    CspDirective? ManifestSrc = null,
    CspDirective? FrameSrc = null,
    CspDirective? ChildSrc = null,
    CspDirective? FormAction = null,
    CspDirective? FrameAncestors = null,
    CspDirective? WorkerSrc = null,
    CspDirective? NavigationTo = null,
    CspSandboxRules? Sandbox = null,
    IReadOnlyCollection<string>? PluginTypes = null,
    CspRequireSRIResources? RequireSriFor = null,
    bool UpgradeInsecureRequests = true,
    bool BlockAllMixedContent = true,
    string? ReportTo = null,
    [property: Obsolete] Uri? ReportUri = null
)
{
    /// <summary>
    /// Initializes a new instance with <c>default-src 'none'</c>.
    /// </summary>
    public CspDirectiveList()
        : this(DefaultSrc: CspDirective.None)
    { }
    public override string ToString()
    {
        ICollection<string> directives = new List<string>();

        if (this.DefaultSrc != null) directives.Add($"default-src {this.DefaultSrc}");
        if (this.BaseUri != null) directives.Add($"base-uri {this.BaseUri}");
        if (this.ChildSrc != null) directives.Add($"child-src {this.ChildSrc}");
        if (this.ConnectSrc != null) directives.Add($"connect-src {this.ConnectSrc}");
        if (this.FontSrc != null) directives.Add($"font-src {this.FontSrc}");
        if (this.FormAction != null) directives.Add($"form-action {this.FormAction}");
        if (this.FrameAncestors != null) directives.Add($"frame-ancestors {this.FrameAncestors}");
        if (this.FrameSrc != null) directives.Add($"frame-src {this.FrameSrc}");
        if (this.ImgSrc != null) directives.Add($"img-src {this.ImgSrc}");
        if (this.ManifestSrc != null) directives.Add($"manifest-src {this.ManifestSrc}");
        if (this.MediaSrc != null) directives.Add($"media-src {this.MediaSrc}");
        if (this.NavigationTo != null) directives.Add($"navigation-to {this.NavigationTo}");
        if (this.ObjectSrc != null) directives.Add($"object-src {this.ObjectSrc}");
        if (this.PrefetchSrc != null) directives.Add($"prefetch-src {this.PrefetchSrc}");
        if (this.ScriptSrc != null) directives.Add($"script-src {this.ScriptSrc}");
        if (this.ScriptSrcAttr != null) directives.Add($"script-src-attr {this.ScriptSrcAttr}");
        if (this.ScriptSrcElem != null) directives.Add($"script-src-elem {this.ScriptSrcElem}");
        if (this.StyleSrc != null) directives.Add($"style-src {this.StyleSrc}");
        if (this.StyleSrcAttr != null) directives.Add($"style-src-attr {this.StyleSrcAttr}");
        if (this.StyleSrcElem != null) directives.Add($"style-src-elem {this.StyleSrcElem}");
        if (this.WorkerSrc != null) directives.Add($"worker-src {this.WorkerSrc}");

        if (this.BlockAllMixedContent) directives.Add("block-all-mixed-content");
        if (this.UpgradeInsecureRequests) directives.Add("upgrade-insecure-requests");
        if (this.PluginTypes?.Any() ?? false) directives.Add($"plugin-types {String.Join(" ", this.PluginTypes)}");
        if (this.RequireSriFor != null) directives.Add(ConstructHeaderValue(this.RequireSriFor.Value));
        if (this.Sandbox != null) directives.Add(ConstructHeaderValue(this.Sandbox.Value));

        if (this.ReportTo != null) directives.Add($"report-to ${this.ReportTo}");
#pragma warning disable CS0612
        if (this.ReportUri != null) directives.Add($"report-uri {this.ReportUri}");
#pragma warning restore CS0612

        return String.Join("; ", directives);
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
