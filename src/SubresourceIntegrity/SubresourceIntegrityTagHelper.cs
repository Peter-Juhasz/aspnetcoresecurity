using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PeterJuhasz.AspNetCore.Extensions.Security.NoOpener
{
    /// <summary>
    /// Adds &quot;noopener&quot; option to the rel attribute when it is not present.
    /// </summary>
    [HtmlTargetElement("script", Attributes = "src")]
    [HtmlTargetElement("link", Attributes = "href,[rel='stylesheet']")]
    public class SubresourceIntegrityTagHelper : TagHelper
    {
        public SubresourceIntegrityTagHelper(
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory,
            ILogger<SubresourceIntegrityTagHelper> logger
        )
        {
            MemoryCache = memoryCache;
            HttpClientFactory = httpClientFactory;
            Logger = logger;
        }

        protected IMemoryCache MemoryCache { get; }
        protected IHttpClientFactory HttpClientFactory { get; }
        protected ILogger<SubresourceIntegrityTagHelper> Logger { get; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            // ignore, if already has integrity
            if (context.AllAttributes.ContainsName("integrity"))
            {
                return;
            }

            // get resource address
            var attribute = context.TagName switch
            {
                "script" => context.AllAttributes["src"],
                "link" => context.AllAttributes["href"],
                _ => throw new NotSupportedException($"Tag '{context.TagName}' is not supported for sub-resource integrity.")
            };

            var content = attribute.Value switch
            {
                string value => value,
                HtmlString htmlString => htmlString.Value,
                _ => null
            };

            if (content == null)
            {
                return;
            }

            if (content.StartsWith("//", StringComparison.OrdinalIgnoreCase))
            {
                content = ViewContext.HttpContext.Request.Scheme + ":" + content;
            }

            // ignore same site
            if (!content.Contains("://", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // ignore malformed URIs
            if (!Uri.TryCreate(content, UriKind.Absolute, out var uri))
            {
                return;
            }

            // calculate hash or get from cache
            var key = $"SRI_{content}";
            var integrity = await MemoryCache.GetOrCreateAsync(key, async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(DateTimeOffset.MaxValue);
                cacheEntry.SetPriority(CacheItemPriority.High);
                using var http = HttpClientFactory.CreateClient();
                try
                {
                    using var stream = await http.GetStreamAsync(uri);
                    using var hashAlgorithm = SHA256.Create();
                    var hash = hashAlgorithm.ComputeHash(stream);
                    var encoded = Convert.ToBase64String(hash);
                    var value = $"sha256-{encoded}";
                    return value;
                }
                catch (HttpRequestException ex)
                {
                    Logger.LogWarning(ex, ex.Message);
                    return null;
                }
            });

            if (integrity == null)
            {
                return;
            }

            output.Attributes.Add("integrity", integrity);
            output.Attributes.Add("crossorigin", "anonymous");
        }
    }
}
