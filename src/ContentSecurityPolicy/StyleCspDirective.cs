using System;
using System.Collections.Generic;

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

        public StyleCspDirective AddSelf()
        {
            return this.AddSource("'self'");
        }

        public StyleCspDirective AddHttpsScheme()
        {
            return this.AddSource("https:");
        }


        public StyleCspDirective AddUnsafeInline()
        {
            return this.AddSource("'unsafe-inline'");
        }

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

        public StyleCspDirective AddHash(CspHashAlgorithm algorithm, string base64Nonce)
        {
            if (base64Nonce == null)
                throw new ArgumentNullException(nameof(base64Nonce));

            return this.AddSource($"{algorithm.ToString().ToLower()}-{base64Nonce}");
        }
        public StyleCspDirective AddHash(CspHashAlgorithm algorithm, byte[] hash)
        {
            return this.AddHash(algorithm, Convert.ToBase64String(hash));
        }
    }
}
