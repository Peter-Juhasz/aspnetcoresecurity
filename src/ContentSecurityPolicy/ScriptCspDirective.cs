using System;
using System.Collections.Generic;
using System.Text;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

public class ScriptCspDirective : CspDirectiveBase
{
    public ScriptCspDirective(IReadOnlyCollection<string> sources)
        : base(sources)
    { }
    public ScriptCspDirective(params string[] sources)
        : this(sources as IReadOnlyCollection<string>)
    { }

    private static readonly ScriptCspDirective _none = new ScriptCspDirective(NoneString);
    private static readonly ScriptCspDirective _self = new ScriptCspDirective(SelfString);

    public static ScriptCspDirective None => _none;
    public static ScriptCspDirective Self => _self;
    public static ScriptCspDirective Empty => new ScriptCspDirective();


    public ScriptCspDirective AddSource(string source)
    {
        return new ScriptCspDirective(base.TryAddSource(source));
    }

    public ScriptCspDirective AddSource(Uri uri)
    {
        if (uri == null)
            throw new ArgumentNullException(nameof(uri));

        return this.AddSource(uri.ToString());
    }

    public ScriptCspDirective AddSelf() => this.AddSource("'self'");

    public ScriptCspDirective AddHttpsScheme() => this.AddSource("https:");


    public ScriptCspDirective AddUnsafeInline() => this.AddSource("'unsafe-inline'");

    public ScriptCspDirective AddUnsafeEval() => this.AddSource("'unsafe-eval'");

    public ScriptCspDirective AddStrictDynamic() => this.AddSource("'strict-dynamic'");


    public ScriptCspDirective AddNonce(string base64Nonce)
    {
        if (base64Nonce == null)
            throw new ArgumentNullException(nameof(base64Nonce));

        return this.AddSource($"'nonce-{base64Nonce}'");
    }

    public ScriptCspDirective AddNonce(byte[] nonce)
    {
        return this.AddNonce(Convert.ToBase64String(nonce));
    }


    public ScriptCspDirective AddHash(CspHashAlgorithm algorithm, byte[] hash)
    {
        if (hash == null)
            throw new ArgumentNullException(nameof(hash));

        string algorithmName = algorithm.ToString().ToLowerInvariant();
        string base64 = Convert.ToBase64String(hash);
        return this.AddSource($"'{algorithmName}-{base64}'");
    }

    public ScriptCspDirective AddHash(CspHashAlgorithm algorithm, string base64hash)
    {
        if (base64hash == null)
            throw new ArgumentNullException(nameof(base64hash));

        string algorithmName = algorithm.ToString().ToLowerInvariant();
        return this.AddSource($"'{algorithmName}-{base64hash}'");
    }

    public ScriptCspDirective AddHashOf(byte[] content, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        using (var hashAlgorithm = algorithm.CreateHashAlgorithm())
            return this.AddHash(algorithm, hashAlgorithm.ComputeHash(content));
    }

    public ScriptCspDirective AddHashOf(string content, Encoding encoding, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        if (encoding == null)
            throw new ArgumentNullException(nameof(encoding));

        byte[] rawContent = encoding.GetBytes(content);
        using (var hashAlgorithm = algorithm.CreateHashAlgorithm())
            return this.AddHash(algorithm, hashAlgorithm.ComputeHash(rawContent));
    }

    public ScriptCspDirective AddHashOf(string content, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256) => this.AddHashOf(content, Encoding.UTF8, algorithm);
}
