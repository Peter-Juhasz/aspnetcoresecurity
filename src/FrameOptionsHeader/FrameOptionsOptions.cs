using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class FrameOptionsOptions
    {
        public FrameOptionsOptions(FrameOptionsPolicy policy = FrameOptionsPolicy.Deny)
        {
            this.Policy = policy;
        }
        public FrameOptionsOptions(Uri allowFromUri)
        {
            this.AllowFrom(allowFromUri);
        }

        public FrameOptionsPolicy Policy { get; private set; } = FrameOptionsPolicy.Deny;

        public Uri AllowFromUri { get; private set; }


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

        public void AllowFrom(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            this.Policy = FrameOptionsPolicy.AllowFrom;
            this.AllowFromUri = uri;
        }


        public override string ToString()
        {
            switch (this.Policy)
            {
                case FrameOptionsPolicy.Deny:
                    return "DENY";
                case FrameOptionsPolicy.SameOrigin:
                    return "SAMEORIGIN";
                case FrameOptionsPolicy.AllowFrom:
                    return $"ALLOW-FROM {this.AllowFromUri}";

                default: throw new NotSupportedException($"Not supported Frame-Options policy '{this.Policy}'.");
            }
        }
    }
}
