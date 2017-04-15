using System;
using System.Collections.Generic;
using System.Linq;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class XRobotsTagHeaderValue
    {
        /// <summary>
        /// Directives can be used to control indexing and serving.
        /// </summary>
        public RobotsTagDirectives? Directives { get; set; } = RobotsTagDirectives.All;

        /// <summary>
        /// Do not show this page in search results after the specified date/time.
        /// </summary>
        public DateTimeOffset? UnavailableAfter { get; set; }


        public override string ToString()
        {
            ICollection<string> directives = new HashSet<string>();

            if (!this.Directives.HasValue ||
                this.Directives.Value == default(RobotsTagDirectives) ||
                this.Directives.Value.HasFlag(RobotsTagDirectives.All))
            {
                directives.Add("all");
            }
            else
            {
                foreach (var directive in Enum.GetValues(typeof(RobotsTagDirectives)).Cast<RobotsTagDirectives>()
                    .Where(d => this.Directives.Value.HasFlag(d))
                )
                    directives.Add(directive.ToString().ToLowerInvariant());
            }

            if (this.UnavailableAfter != null)
                directives.Add($"unavailable_after: {this.UnavailableAfter.Value.UtcDateTime:dddd, d-MMM-yyyy H:mm:ss UTC}");

            return String.Join(", ", directives);
        }
    }
}
