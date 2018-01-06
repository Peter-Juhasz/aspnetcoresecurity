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

		/// <summary>
		/// Serves as a fallback for the other fetch directives.
		/// </summary>
		public CspDirective DefaultSrc { get; set; } = CspDirective.None;

		/// <summary>
		/// Specifies valid sources for stylesheets.
		/// </summary>
		public StyleCspDirective StyleSrc { get; set; }

		/// <summary>
		/// Specifies valid sources for JavaScript.
		/// </summary>
		public ScriptCspDirective ScriptSrc { get; set; }

		/// <summary>
		/// Specifies valid sources of images and favicons
		/// </summary>
		public CspDirective ImgSrc { get; set; }

		/// <summary>
		/// Specifies valid sources for fonts loaded using @font-face.
		/// </summary>
		public CspDirective FontSrc { get; set; }

		/// <summary>
		/// Restricts the URLs which can be loaded using script interfaces.
		/// </summary>
		public CspDirective ConnectSrc { get; set; }

		/// <summary>
		/// Specifies valid sources for the &lt;object&gt;, &lt;embed&gt;, and &lt;applet&gt; elements.
		/// </summary>
		public CspDirective ObjectSrc { get; set; }

		/// <summary>
		/// Specifies valid sources for loading media using the &lt;audio&gt; and &lt;video&gt; elements.
		/// </summary>
		public CspDirective MediaSrc { get; set; }

		/// <summary>
		/// Specifies valid sources of application manifest files.
		/// </summary>
		public CspDirective ManifestSrc { get; set; }

		/// <summary>
		/// Specifies valid sources for nested browsing contexts loading using elements such as &lt;frame&gt; and &lt;iframe&gt;.
		/// </summary>
		public CspDirective FrameSrc { get; set; }

		/// <summary>
		/// Defines the valid sources for web workers and nested browsing contexts loaded using elements such as &lt;frame&gt; and &lt;iframe&gt;.
		/// </summary>
		public CspDirective ChildSrc { get; set; }

		/// <summary>
		/// Restricts the URLs which can be used as the target of a form submissions from a given context.
		/// </summary>
		public CspDirective FormAction { get; set; }

		/// <summary>
		/// Specifies valid parents that may embed a page using &lt;frame&gt;, &lt;iframe&gt;, &lt;object&gt;, &lt;embed&gt;, or &lt;applet&gt;.
		/// </summary>
		public CspDirective FrameAncestors { get; set; }

		/// <summary>
		/// Specifies valid sources for Worker, SharedWorker, or ServiceWorker scripts.
		/// </summary>
		public CspDirective WorkerSrc { get; set; }

		/// <summary>
		/// Restricts the URLs to which a document can navigate by any means (a, form, window.location, window.open, etc.).
		/// </summary>
		public CspDirective NavigationTo { get; set; }

		/// <summary>
		/// Enables a sandbox for the requested resource similar to the &lt;iframe&gt; sandbox attribute.
		/// </summary>
		public CspSandboxRules? Sandbox { get; set; }

		/// <summary>
		/// Restricts the set of plugins that can be embedded into a document by limiting the types of resources which can be loaded.
		/// </summary>
		public IReadOnlyCollection<string> PluginTypes { get; set; }

		/// <summary>
		/// Requires the use of SRI for scripts or styles on the page.
		/// </summary>
		public CspRequireSRIResources? RequireSriFor { get; set; }

		/// <summary>
		/// Instructs a user agent to activate or deactivate any heuristics used to filter or block reflected cross-site scripting attacks, equivalent to the effects of the non-standard X-XSS-Protection header.
		/// </summary>
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

		public CspOptions Clone()
		{
			return new CspOptions
			{
				BaseUri = this.BaseUri,
				BlockAllMixedContent = this.BlockAllMixedContent,
				ChildSrc = this.ChildSrc,
				ConnectSrc = this.ConnectSrc,
				DefaultSrc = this.DefaultSrc,
				FontSrc = this.FontSrc,
				FormAction = this.FormAction,
				FrameAncestors = this.FrameAncestors,
				FrameSrc = this.FrameSrc,
				ImgSrc = this.ImgSrc,
				ManifestSrc = this.ManifestSrc,
				MediaSrc = this.MediaSrc,
				NavigationTo = this.NavigationTo,
				ObjectSrc = this.ObjectSrc,
				PluginTypes = this.PluginTypes,
				ReflectedXss = this.ReflectedXss,
				RequireSriFor = this.RequireSriFor,
				Sandbox = this.Sandbox,
				ScriptSrc = this.ScriptSrc,
				StyleSrc = this.StyleSrc,
				UpgradeInsecureRequests = this.UpgradeInsecureRequests,
				WorkerSrc = this.WorkerSrc,
#pragma warning disable CS0612
				ReportUri = this.ReportUri,
#pragma warning restore CS0612
			};
		}

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
