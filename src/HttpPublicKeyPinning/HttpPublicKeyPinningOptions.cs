using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents a pinned public key fingerprint.
/// </summary>
public record class HttpPublicKeyPin(HttpPublicKeyPinningHashAlgorithm Algorithm, string Base64Fingerprint);

/// <summary>
/// Represents the options of the Public-Key-Pins header.
/// </summary>
/// <param name="MaxAge">The time, that the browser should remember that this site is only to be accessed using one of the pinned keys.</param>
/// <param name="Pins">The pinned keys.</param>
/// <param name="IncludeSubDomains">Whether this rule applies to all of the site's subdomains as well.</param>
/// <param name="ReportUri">The URI pin validation failures are reported to.</param>
public record class HttpPublicKeyPinningOptions(
    TimeSpan MaxAge,
    IReadOnlyList<HttpPublicKeyPin> Pins,
    bool IncludeSubDomains = true,
    Uri? ReportUri = null
)
{

    /// <summary>
    /// Returns a copy with the fingerprint of a certificate added to the pinned keys.
    /// </summary>
    public HttpPublicKeyPinningOptions Pin(string base64Fingerprint, HttpPublicKeyPinningHashAlgorithm algorithm = HttpPublicKeyPinningHashAlgorithm.Sha256)
    {
        ArgumentNullException.ThrowIfNull(base64Fingerprint);
        return this with { Pins = [.. Pins, new(Algorithm: algorithm, Base64Fingerprint: base64Fingerprint)] };
    }

    /// <summary>
    /// Returns a copy with the fingerprint of a certificate added to the pinned keys.
    /// </summary>
    public HttpPublicKeyPinningOptions Pin(byte[] fingerprint, HttpPublicKeyPinningHashAlgorithm algorithm = HttpPublicKeyPinningHashAlgorithm.Sha256)
    {
        return Pin(Convert.ToBase64String(fingerprint), algorithm);
    }


    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var item in Pins)
        {
            if (builder.Length > 0)
                builder.Append("; ");

            builder.Append($"pin-{item.Algorithm.ToString().ToLowerInvariant()}=\"{item.Base64Fingerprint}\"");
        }

        builder.Append($"; max-age={MaxAge.TotalSeconds:F0}");

        if (IncludeSubDomains)
            builder.Append("; includeSubDomains");

        if (ReportUri is not null)
            builder.Append($"; report-uri=\"{ReportUri}\"");

        return builder.ToString();
    }
}
