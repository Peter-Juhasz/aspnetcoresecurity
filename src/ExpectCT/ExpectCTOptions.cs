using System;
using System.Collections.Generic;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents the options of the Expect-CT header.
/// </summary>
/// <param name="Enforce">Whether to enforce the policy or keep it in report-only mode.</param>
/// <param name="MaxAge">The time that the browser should cache and apply the received policy.</param>
/// <param name="ReportUri">The URI the browser is going to report violations to.</param>
public record class ExpectCTOptions(bool Enforce = true, TimeSpan? MaxAge = null, Uri? ReportUri = null)
{
    public override string ToString()
    {
        var directives = new List<string>();

        if (Enforce)
            directives.Add("enforce");

        if (MaxAge is { } maxAge)
            directives.Add($"max-age={(int)maxAge.TotalSeconds}");

        if (ReportUri is not null)
            directives.Add($"report-uri={ReportUri}");

        return string.Join(", ", directives);
    }
}
