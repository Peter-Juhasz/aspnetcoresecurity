using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents the value of the X-Frame-Options header.
/// </summary>
/// <param name="Policy">The framing policy.</param>
/// <param name="AllowFromUri">The allowed origin, used only with <see cref="FrameOptionsPolicy.AllowFrom"/>.</param>
public record class FrameOptionsDirective(
    FrameOptionsPolicy Policy = FrameOptionsPolicy.Deny,
    Uri? AllowFromUri = null
)
{
#pragma warning disable CS0612 // Type or member is obsolete
    public override string ToString() => Policy switch
    {
        FrameOptionsPolicy.AllowFrom => $"ALLOW-FROM {AllowFromUri!}",
        _ => ToString(Policy),
    };
#pragma warning restore CS0612 // Type or member is obsolete

    public static string ToString(FrameOptionsPolicy policy) => policy switch
    {
        FrameOptionsPolicy.Deny => "DENY",
        FrameOptionsPolicy.SameOrigin => "SAMEORIGIN",
        _ => throw new NotSupportedException($"Not supported Frame-Options policy '{policy}'."),
    };
}
