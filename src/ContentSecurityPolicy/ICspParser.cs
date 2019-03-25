namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    /// <summary>
    /// A Content Security Policy restricts which resources a web page can load, protecting against risks such as cross-site scripting.
    /// </summary>
    public interface ICspParser
    {
        /// <summary>
        /// Parses a Content Security Policy HTTP header value as an instance of <see cref="CspOptions"/>.
        /// </summary>
        /// <param name="policy">The policy.</param>
        CspOptions ParsePolicy(string policy);
    }
}