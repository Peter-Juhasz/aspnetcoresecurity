using System;
using System.Collections.Generic;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
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

        public CspDirective AddSelf()
        {
            return this.AddSource("'self'");
        }

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

        public CspDirective AddDataScheme()
        {
            return this.AddScheme("data");
        }
        public CspDirective AddBlobScheme()
        {
            return this.AddScheme("blob");
        }
        public CspDirective AddHttpsScheme()
        {
            return this.AddScheme("https");
        }
        public CspDirective AddMediaStreamScheme()
        {
            return this.AddScheme("mediastream");
        }
        public CspDirective AddFileSystemScheme()
        {
            return this.AddScheme("filesystem");
        }
    }
}
