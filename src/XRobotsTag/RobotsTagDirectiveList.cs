using System;
using System.Collections.Generic;
using System.Linq;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

public class RobotsTagDirectiveList
{
    /// <summary>
    /// Use a maximum of [number] characters as a textual snippet for this search result.
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Directives can be used to control indexing and serving.
    /// </summary>
    public RobotsTagDirectives? Directives { get; set; } = RobotsTagDirectives.All;

    /// <summary>
    /// Do not show this page in search results after the specified date/time.
    /// </summary>
    public DateTimeOffset? UnavailableAfter { get; set; }

    /// <summary>
    /// Use a maximum of [number] characters as a textual snippet for this search result.
    /// </summary>
    public int? MaxSnippet { get; set; }

    /// <summary>
    /// Use a maximum of [number] seconds as a video snippet for videos on this page in search results.
    /// </summary>
    public int? MaxVideoPreview { get; set; }

    /// <summary>
    /// Set the maximum size of an image preview for this page in a search results.
    /// </summary>
    public RobotsTxtImagePreviewSize? MaxImagePreview { get; set; }

    public override string ToString()
    {
        ICollection<string> directives = new HashSet<string>();

        if (this.Directives == null ||
            this.Directives.Value == default ||
            this.Directives.Value.HasFlag(RobotsTagDirectives.All))
        {
            directives.Add("all");
        }
        else
        {
            foreach (var directive in Enum.GetValues(typeof(RobotsTagDirectives)).Cast<RobotsTagDirectives>()
                .Where(d => this.Directives.Value.HasFlag(d))
            )
                directives.Add(directive.ToString().ToLowerInvariant());
        }

        if (this.UnavailableAfter != null)
            directives.Add($"unavailable_after: {this.UnavailableAfter.Value.UtcDateTime:dddd, d-MMM-yyyy H:mm:ss UTC}");

        if (this.MaxSnippet != null)
            directives.Add($"max-snippet:{MaxSnippet}");

        if (this.MaxVideoPreview != null)
            directives.Add($"max-video-preview:{MaxVideoPreview}");

        if (this.MaxImagePreview != null)
            directives.Add($"max-image-preview:{MaxImagePreview.Value.ToString().ToLowerInvariant()}");

        if (this.UserAgent != null)
        {
            return $"{UserAgent}: {String.Join(", ", directives)}";
        }

        return String.Join(", ", directives);
    }
}
