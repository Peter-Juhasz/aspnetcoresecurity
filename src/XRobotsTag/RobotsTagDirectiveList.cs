using System;
using System.Collections.Generic;
using System.Linq;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents the value of the X-Robots-Tag header.
/// </summary>
/// <param name="UserAgent">The user agent the directives apply to.</param>
/// <param name="Directives">Directives can be used to control indexing and serving.</param>
/// <param name="UnavailableAfter">Do not show this page in search results after the specified date/time.</param>
/// <param name="MaxSnippet">Use a maximum of [number] characters as a textual snippet for this search result.</param>
/// <param name="MaxVideoPreview">Use a maximum of [number] seconds as a video snippet for videos on this page in search results.</param>
/// <param name="MaxImagePreview">Set the maximum size of an image preview for this page in a search results.</param>
public record class RobotsTagDirectiveList(
    string? UserAgent = null,
    RobotsTagDirectives? Directives = RobotsTagDirectives.All,
    DateTimeOffset? UnavailableAfter = null,
    int? MaxSnippet = null,
    int? MaxVideoPreview = null,
    RobotsTxtImagePreviewSize? MaxImagePreview = null
)
{
    public override string ToString()
    {
        var directives = new HashSet<string>();

        if (Directives is not { } flags || flags == default || flags.HasFlag(RobotsTagDirectives.All))
        {
            directives.Add("all");
        }
        else
        {
            foreach (var directive in Enum.GetValues<RobotsTagDirectives>().Where(d => flags.HasFlag(d)))
                directives.Add(directive.ToString().ToLowerInvariant());
        }

        if (UnavailableAfter is { } unavailableAfter)
            directives.Add($"unavailable_after: {unavailableAfter.UtcDateTime:dddd, d-MMM-yyyy H:mm:ss UTC}");

        if (MaxSnippet is { } maxSnippet)
            directives.Add($"max-snippet:{maxSnippet}");

        if (MaxVideoPreview is { } maxVideoPreview)
            directives.Add($"max-video-preview:{maxVideoPreview}");

        if (MaxImagePreview is { } maxImagePreview)
            directives.Add($"max-image-preview:{maxImagePreview.ToString().ToLowerInvariant()}");

        var value = string.Join(", ", directives);

        return UserAgent is null ? value : $"{UserAgent}: {value}";
    }
}
