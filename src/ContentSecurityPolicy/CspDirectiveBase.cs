using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public abstract class CspDirectiveBase
    {
        public CspDirectiveBase(IReadOnlyCollection<string> sources)
        {
            this.Sources = sources ?? throw new ArgumentNullException(nameof(sources));
        }
        public CspDirectiveBase(params string[] sources)
            : this(sources as IReadOnlyCollection<string>)
        { }

        internal const string NoneString = "'none'";
        internal const string SelfString = "'self'";


        internal protected IReadOnlyCollection<string> Sources { get; private set; }

        protected IReadOnlyCollection<string> TryAddSource(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (this.Sources.Contains("'none'"))
                throw new InvalidOperationException($"Can't add any sources if {NoneString} is present. Use {nameof(CspDirective.Empty)} as a starting point.");

            if (source == NoneString && !this.Sources.Any())
                throw new InvalidOperationException($"Can't add {NoneString} if any non-none sources are already configured.");

            if (this.Sources.Contains(source))
                return this.Sources;

            return new ReadOnlyCollection<string>(this.Sources.Append(source).ToList());
        }

        public override string ToString()
        {
            return String.Join(" ", this.Sources);
        }
    }
}
