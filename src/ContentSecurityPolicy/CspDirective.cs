using System;
using System.Collections.Generic;
using System.Text;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

public class CspDirective : CspDirectiveBase
{
    public CspDirective(IReadOnlyCollection<string> sources)
        : base(sources)
    { }
    public CspDirective(params string[] sources)
        : this(sources as IReadOnlyCollection<string>)
    { }

    private static readonly CspDirective _none = new CspDirective(NoneString);
    private static readonly CspDirective _self = new CspDirective(SelfString);

    public static CspDirective None => _none;
    public static CspDirective Self => _self;
    public static CspDirective Empty => new CspDirective();


    public CspDirective AddSource(string source)
    {
        return new CspDirective(base.TryAddSource(source));
    }

    public CspDirective AddSelf() => this.AddSource("'self'");

    public CspDirective AddSource(Uri uri)
    {
        if (uri == null)
            throw new ArgumentNullException(nameof(uri));

        return this.AddSource(uri.ToString());
    }

    public CspDirective AddScheme(string scheme)
    {
        if (scheme == null)
            throw new ArgumentNullException(nameof(scheme));

        return this.AddSource($"{scheme}:");
    }

    public CspDirective AddDataScheme() => this.AddScheme("data");
    public CspDirective AddBlobScheme() => this.AddScheme("blob");
    public CspDirective AddHttpsScheme() => this.AddScheme("https");
    public CspDirective AddMediaStreamScheme() => this.AddScheme("mediastream");
    public CspDirective AddFileSystemScheme() => this.AddScheme("filesystem");

    public CspDirective AddHash(CspHashAlgorithm algorithm, byte[] hash)
    {
        if (hash == null)
            throw new ArgumentNullException(nameof(hash));

        string algorithmName = algorithm.ToString().ToLowerInvariant();
        string base64 = Convert.ToBase64String(hash);
        return this.AddSource($"'{algorithmName}-{base64}'");
    }

    public CspDirective AddHash(CspHashAlgorithm algorithm, string base64hash)
    {
        if (base64hash == null)
            throw new ArgumentNullException(nameof(base64hash));

        string algorithmName = algorithm.ToString().ToLowerInvariant();
        return this.AddSource($"'{algorithmName}-{base64hash}'");
    }

    public CspDirective AddHashOf(byte[] content, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        using (var hashAlgorithm = algorithm.CreateHashAlgorithm())
            return this.AddHash(algorithm, hashAlgorithm.ComputeHash(content));
    }

    public CspDirective AddHashOf(string content, Encoding encoding, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        if (encoding == null)
            throw new ArgumentNullException(nameof(encoding));

        byte[] rawContent = encoding.GetBytes(content);
        using (var hashAlgorithm = algorithm.CreateHashAlgorithm())
            return this.AddHash(algorithm, hashAlgorithm.ComputeHash(rawContent));
    }

    public CspDirective AddHashOf(string content, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256) => this.AddHashOf(content, Encoding.UTF8, algorithm);
}
