using System;
using System.Text;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class HttpStrictTransportSecurityOptions
    {
        /// <summary>
        /// The time, that the browser should remember that this site is only to be accessed using HTTPS.
        /// </summary>
        public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(365);

        /// <summary>
        /// If this optional parameter is specified, this rule applies to all of the site's subdomains as well.
        /// </summary>
        public bool IncludeSubDomains { get; set; } = true;

        /// <summary>
        /// If a site sends the preload directive in an HSTS header, it is considered to be requesting inclusion in the preload list.
        /// </summary>
        public bool Preload { get; set; } = true;


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"max-age={this.MaxAge.TotalSeconds:F0}");

            if (this.IncludeSubDomains)
                builder.Append("; includeSubDomains");

            if (this.Preload)
                builder.Append("; preload");

            return builder.ToString();
        }
    }
}
