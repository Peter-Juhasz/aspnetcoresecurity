using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security.NoOpener
{
    /// <summary>
    /// Adds &quot;noopener&quot; option to the rel attribute when it is not present.
    /// </summary>
    [HtmlTargetElement("a", Attributes = "href,target")]
    public class NoOpenerTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override int Order => Int32.MaxValue;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (context.AllAttributes.TryGetAttribute("href", out var attribute))
            {
                var content = attribute.Value switch
                {
                    string value => value,
                    HtmlString htmlString => htmlString.Value,
                    _ => null
                };

                if (content != null)
                {
                    if (content.StartsWith("//", StringComparison.OrdinalIgnoreCase))
                    {
                        content = ViewContext.HttpContext.Request.Scheme + ":" + content;
                    }

                    // ignore same site
                    if (!content.Contains("://", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    // ignore same site
                    if (Uri.TryCreate(content, UriKind.Absolute, out var uri))
                    {
                        var host = HostString.FromUriComponent(uri);
                        if (host == ViewContext.HttpContext.Request.Host)
                        {
                            return;
                        }
                    }
                }
            }

            if (context.AllAttributes.TryGetAttribute("rel", out attribute))
            {
                var content = attribute.Value switch
                {
                    string value => value,
                    HtmlString htmlString => htmlString.Value,
                    _ => null
                };
                
                if (content != null && !content.Contains("noopener", StringComparison.OrdinalIgnoreCase))
                {
                    output.Attributes.SetAttribute("rel", content + " noopener");
                }
            }
            else
            {
                output.Attributes.Add("rel", "noopener");
            }
        }
    }
}
