using System;
using System.Security.Cryptography;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public enum CspHashAlgorithm
    {
        Sha256,
        Sha384,
        Sha512,
    }

    internal static class CspHashAlgorithmExtensions
    {
        public static HashAlgorithm CreateHashAlgorithm(this CspHashAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case CspHashAlgorithm.Sha256: return SHA256.Create();
                case CspHashAlgorithm.Sha384: return SHA384.Create();
                case CspHashAlgorithm.Sha512: return SHA512.Create();

                default: throw new NotSupportedException($"Could not create HashAlgorithm from '{algorithm}'.");
            }
        }
    }
}
