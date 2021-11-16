using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

public class FrameOptionsDirective
{
    public FrameOptionsDirective(FrameOptionsPolicy policy = FrameOptionsPolicy.Deny)
    {
        this.Policy = policy;
    }

    [Obsolete]
    public FrameOptionsDirective(Uri allowFromUri)
    {
        this.AllowFrom(allowFromUri);
    }

    public FrameOptionsPolicy Policy { get; private set; } = FrameOptionsPolicy.Deny;

    public Uri? AllowFromUri { get; private set; }


    public void Deny()
    {
        this.Policy = FrameOptionsPolicy.Deny;
        this.AllowFromUri = null;
    }

    public void SameOrigin()
    {
        this.Policy = FrameOptionsPolicy.SameOrigin;
        this.AllowFromUri = null;
    }

    [Obsolete]
    public void AllowFrom(Uri uri)
    {
        this.Policy = FrameOptionsPolicy.AllowFrom;
        this.AllowFromUri = uri;
    }

    [Obsolete]
    public override string ToString() => this.Policy switch
    {
        FrameOptionsPolicy.AllowFrom => $"ALLOW-FROM {this.AllowFromUri!}",
        _ => ToString(Policy),
    };

    public static string ToString(FrameOptionsPolicy policy) => policy switch
    {
        FrameOptionsPolicy.Deny => "DENY",
        FrameOptionsPolicy.SameOrigin => "SAMEORIGIN",
        _ => throw new NotSupportedException($"Not supported Frame-Options policy '{policy}'."),
    };
}
