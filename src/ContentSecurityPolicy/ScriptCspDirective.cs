using System;
using System.Collections.Generic;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
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

        public ScriptCspDirective AddSelf()
        {
            return this.AddSource("'self'");
        }

        public ScriptCspDirective AddHttpsScheme()
        {
            return this.AddSource("https:");
        }


        public ScriptCspDirective AddUnsafeInline()
        {
            return this.AddSource("'unsafe-inline'");
        }

        public ScriptCspDirective AddUnsafeEval()
        {
            return this.AddSource("'unsafe-eval'");
        }

        public ScriptCspDirective AddStrictDynamic()
        {
            return this.AddSource("'strict-dynamic'");
        }


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


        public ScriptCspDirective AddHash(CspHashAlgorithm algorithm, string base64Hash)
        {
            if (base64Hash == null)
                throw new ArgumentNullException(nameof(base64Hash));

            return this.AddSource($"{algorithm.ToString().ToLower()}-{base64Hash}");
        }
        public ScriptCspDirective AddHash(CspHashAlgorithm algorithm, byte[] hash)
        {
            return this.AddHash(algorithm, Convert.ToBase64String(hash));
        }
    }
}
