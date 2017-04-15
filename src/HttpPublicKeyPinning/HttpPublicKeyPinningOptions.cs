using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class HttpPublicKeyPinningOptions
    {
        internal ICollection<Pinning> Pins { get; private set; } = new Collection<Pinning>();

        public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(365);

        public bool IncludeSubDomains { get; set; } = true;

        public Uri ReportUri { get; set; }


        public HttpPublicKeyPinningOptions Pin(string base64Fingerprint, HttpPublicKeyPinningHashAlgorithm algorithm = HttpPublicKeyPinningHashAlgorithm.Sha256)
        {
            this.Pins.Add(new Pinning(algorithm, base64Fingerprint));
            return this;
        }

        public HttpPublicKeyPinningOptions Pin(byte[] fingerprint, HttpPublicKeyPinningHashAlgorithm algorithm = HttpPublicKeyPinningHashAlgorithm.Sha256)
        {
            return this.Pin(Convert.ToBase64String(fingerprint), algorithm);
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
