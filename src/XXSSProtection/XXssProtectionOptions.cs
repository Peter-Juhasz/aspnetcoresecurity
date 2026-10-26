using System;
using System.Text;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents the options of the X-XSS-Protection header.
/// </summary>
/// <param name="Enabled">Whether XSS protection is enabled.</param>
/// <param name="Block">Whether to block the page instead of sanitizing it.</param>
/// <param name="ReportUri">The URI the browser is going to report violations to.</param>
public record class XXssProtectionOptions(bool Enabled = true, bool Block = true, Uri? ReportUri = null)
{
    public override string ToString()
    {
        if (!Enabled)
            return "0";

        var builder = new StringBuilder("1");

        if (Block)
            builder.Append("; mode=block");

        if (ReportUri is not null)
            builder.Append($"report={ReportUri}");

        return builder.ToString();
    }
}
