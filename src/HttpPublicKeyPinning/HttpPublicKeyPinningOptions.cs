using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class HttpPublicKeyPinningOptions
    {
        /// <summary>
        /// The time, that the browser should remember that this site is only to be accessed using one of the pinned keys. 
        /// </summary>
        public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(365);

        /// <summary>
        /// If this optional parameter is specified, this rule applies to all of the site's subdomains as well.
        /// </summary>
        public bool IncludeSubDomains { get; set; } = true;

        /// <summary>
        /// If this optional parameter is specified, pin validation failures are reported to the given URL.
        /// </summary>
        public Uri? ReportUri { get; set; }

        internal ICollection<Pinning> Pins { get; private set; } = new Collection<Pinning>();


        /// <summary>
        /// Adds the fingerprint of a certificate to the pinned keys.
        /// </summary>
        /// <param name="base64Fingerprint"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public HttpPublicKeyPinningOptions Pin(string base64Fingerprint, HttpPublicKeyPinningHashAlgorithm algorithm = HttpPublicKeyPinningHashAlgorithm.Sha256)
        {
            this.Pins.Add(new Pinning(algorithm, base64Fingerprint));
            return this;
        }

        /// <summary>
        /// Adds the fingerprint of a certificate to the pinned keys.
        /// </summary>
        /// <param name="fingerprint"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public HttpPublicKeyPinningOptions Pin(byte[] fingerprint, HttpPublicKeyPinningHashAlgorithm algorithm = HttpPublicKeyPinningHashAlgorithm.Sha256)
        {
            return this.Pin(Convert.ToBase64String(fingerprint), algorithm);
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in this.Pins)
            {
                if (builder.Length > 0)
                    builder.Append("; ");

                builder.Append($"pin-{item.Algorithm.ToString().ToLowerInvariant()}=\"{item.Base64Fingerprint}\"");
            }

            builder.Append($"; max-age={this.MaxAge.TotalSeconds:F0}");

            if (this.IncludeSubDomains)
                builder.Append("; includeSubDomains");

            if (this.ReportUri != null)
                builder.Append($"; report-uri=\"{this.ReportUri}\"");

            return builder.ToString();
        }


        internal sealed class Pinning
        {
            public Pinning(HttpPublicKeyPinningHashAlgorithm algorithm, string base64Fingerprint)
            {
                this.Algorithm = algorithm;
                this.Base64Fingerprint = base64Fingerprint ?? throw new ArgumentNullException(nameof(base64Fingerprint));
            }

            public HttpPublicKeyPinningHashAlgorithm Algorithm { get; private set; }

            public string Base64Fingerprint { get; private set; }
        }
    }
}
