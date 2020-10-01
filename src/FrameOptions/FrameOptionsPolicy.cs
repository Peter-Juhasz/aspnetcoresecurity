using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    /// <summary>
    /// Defines the Frame Options policy.
    /// </summary>
    public enum FrameOptionsPolicy
    {
        /// <summary>
        /// The page cannot be displayed in a frame, regardless of the site attempting to do so.
        /// </summary>
        Deny = 0,

        /// <summary>
        /// The page can only be displayed in a frame on the same origin as the page itself.
        /// </summary>
        SameOrigin,

        /// <summary>
        /// The page can only be displayed in a frame on the specified origin.
        /// </summary>
        [Obsolete]
        AllowFrom,
    }
}
