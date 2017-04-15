using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class XRobotsTagHeaderValue
    {
        public RobotsTagDirectives? Directives { get; set; } = RobotsTagDirectives.All;

        public DateTimeOffset? UnavailableAfter { get; set; }
    }
}
