using System;
using System.Collections.Generic;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class ExpectCTOptions
    {
        /// <summary>
        /// Gets or sets whether enforce policy or keep it in report-only mode.
        /// </summary>
        public bool Enforce { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? MaxAge { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public Uri ReportUri { get; set; }


        public override string ToString()
        {
            IList<string> directives = new List<string>();
            
            if (this.Enforce)
                directives.Add("enforce");

            if (this.MaxAge != null)
                directives.Add($"max-age={(int)this.MaxAge.Value.TotalSeconds}");

            if (this.ReportUri != null)
                directives.Add($"report-uri={this.ReportUri}");
           
            return String.Join(", ", directives);
        }
    }
}
