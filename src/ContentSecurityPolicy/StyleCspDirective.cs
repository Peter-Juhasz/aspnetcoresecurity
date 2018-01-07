using System;
using System.Collections.Generic;
using System.Text;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class StyleCspDirective : CspDirectiveBase
    {
        public StyleCspDirective(IReadOnlyCollection<string> sources)
            : base(sources)
        { }
        public StyleCspDirective(params string[] sources)
            : this(sources as IReadOnlyCollection<string>)
        { }

        private static readonly StyleCspDirective _none = new StyleCspDirective(NoneString);
        private static readonly StyleCspDirective _self = new StyleCspDirective(SelfString);

        public static StyleCspDirective None => _none;
        public static StyleCspDirective Self => _self;
        public static StyleCspDirective Empty => new StyleCspDirective();


        public StyleCspDirective AddSource(string source)
        {
            return new StyleCspDirective(base.TryAddSource(source));
        }

        public StyleCspDirective AddSource(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return this.AddSource(uri.ToString());
        }

        public StyleCspDirective AddSelf() => this.AddSource("'self'");

        public StyleCspDirective AddHttpsScheme() => this.AddSource("https:");


        public StyleCspDirective AddUnsafeInline() => this.AddSource("'unsafe-inline'");


        public StyleCspDirective AddNonce(string base64String)
        {
            if (base64String == null)
                throw new ArgumentNullException(nameof(base64String));

            return this.AddSource($"'nonce-{base64String}'");
        }

        public StyleCspDirective AddNonce(byte[] nonce)
        {
            return this.AddNonce(Convert.ToBase64String(nonce));
        }

        public StyleCspDirective AddHash(CspHashAlgorithm algorithm, byte[] hash)
        {
            if (hash == null)
                throw new ArgumentNullException(nameof(hash));

            string algorithmName = algorithm.ToString().ToLowerInvariant();
            string base64 = Convert.ToBase64String(hash);
            return this.AddSource($"'{algorithmName}-{base64}'");
        }

        public StyleCspDirective AddHash(CspHashAlgorithm algorithm, string base64hash)
        {
            if (base64hash == null)
                throw new ArgumentNullException(nameof(base64hash));

            string algorithmName = algorithm.ToString().ToLowerInvariant();
            return this.AddSource($"'{algorithmName}-{base64hash}'");
        }

        public StyleCspDirective AddHashOf(byte[] content, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            using (var hashAlgorithm = algorithm.CreateHashAlgorithm())
                return this.AddHash(algorithm, hashAlgorithm.ComputeHash(content));
        }

        public StyleCspDirective AddHashOf(string content, Encoding encoding, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            byte[] rawContent = encoding.GetBytes(content);
            using (var hashAlgorithm = algorithm.CreateHashAlgorithm())
                return this.AddHash(algorithm, hashAlgorithm.ComputeHash(rawContent));
        }

        public StyleCspDirective AddHashOf(string content, CspHashAlgorithm algorithm = CspHashAlgorithm.Sha256) => this.AddHashOf(content, Encoding.UTF8, algorithm);
    }
}
