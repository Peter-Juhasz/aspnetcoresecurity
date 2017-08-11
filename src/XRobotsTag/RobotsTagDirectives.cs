using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    [Flags]
    public enum RobotsTagDirectives
    {
        /// <summary>
        /// There are no restrictions for indexing or serving. Note: this directive is the default value and has no effect if explicitly listed.
        /// </summary>
        All = 0,

        /// <summary>
        /// Do not show this page in search results and do not show a "Cached" link in search results.
        /// </summary>
        NoIndex,

        /// <summary>
        /// Do not follow the links on this page.
        /// </summary>
        NoFollow,

        /// <summary>
        /// Equivalent to <see cref="NoIndex"/>, <see cref="NoFollow"/>
        /// </summary>
        None,

        /// <summary>
        /// Do not show a "Cached" link in search results.
        /// </summary>
        NoArchive,

        /// <summary>
        /// Equivalent to <see cref="NoArchive"/>.
        /// </summary>
        NoCache,

        /// <summary>
        /// Do not show a snippet in the search results for this page
        /// </summary>
        NoSnippet,

        /// <summary>
        /// Do not use metadata from the Open Directory project for titles or snippets shown for this page.
        /// </summary>
        NoOdp,

        /// <summary>
        /// Do not offer translation of this page in search results.
        /// </summary>
        NoTranslate,

        /// <summary>
        /// Do not index images on this page.
        /// </summary>
        NoImageIndex,
    }
}
