using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security.UpgradeInsecureResources
{
    /// <summary>
    /// Adds &quot;noopener&quot; option to the rel attribute when it is not present.
    /// </summary>
    [HtmlTargetElement(Attributes = "[href^='http:']")]
    [HtmlTargetElement(Attributes = "[src^='http:']")]
    public class UpgradeInsecureResourcesTagHelper : TagHelper
    {
        public override int Order => Int32.MaxValue - 1;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            Upgrade(context, output, "href");
            Upgrade(context, output, "src");
        }

        private static void Upgrade(TagHelperContext context, TagHelperOutput output, string attributeName)
        {
            if (context.AllAttributes.TryGetAttribute(attributeName, out var attribute))
            {
                var content = attribute.Value switch
                {
                    string value => value,
                    HtmlString htmlString => htmlString.Value,
                    _ => null
                };

                if (content?.StartsWith("http:", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    output.Attributes.SetAttribute(attributeName, content.Insert("http".Length, "s"));
                }
            }
        }
    }
}
