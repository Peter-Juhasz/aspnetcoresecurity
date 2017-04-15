using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class XXssProtectionOptions
    {
        public bool Enabled { get; set; } = true;

        public bool Block { get; set; } = true;

        public Uri ReportUri { get; set; }
    }
}
