namespace Microsoft.AspNetCore.Builder;

public enum ReferrerPolicy
{
    /// <summary>
    /// The Referer header will be omitted entirely. No referrer information is sent along with requests. 
    /// </summary>
    NoReferrer,

    /// <summary>
    /// This is the user agent's default behavior if no policy is specified. The origin is sent as referrer to a-priori as-much-secure destination (HTTPS->HTTPS), but isn't sent to a less secure destination (HTTPS->HTTP). 
    /// </summary>
    NoReferrerWhenDowngrade,

    /// <summary>
    /// Only send the origin of the document as the referrer in all cases. The document https://example.com/page.html will send the referrer https://example.com/. 
    /// </summary>
    SameOrigin,

    /// <summary>
    /// Send a full URL when performing a same-origin request, but only send the origin of the document for other cases. 
    /// </summary>
    Origin,

    /// <summary>
    /// A referrer will be sent for same-site origins, but cross-origin requests will contain no referrer information. 
    /// </summary>
    StrictOrigin,

    /// <summary>
    /// Only send the origin of the document as the referrer to a-priori as-much-secure destination (HTTPS->HTTPS), but don't send it to a less secure destination (HTTPS->HTTP). 
    /// </summary>
    OriginWhenCrossOrigin,

    /// <summary>
    /// Send a full URL when performing a same-origin request, only send the origin of the document to a-priori as-much-secure destination (HTTPS->HTTPS), and send no header to a less secure destination (HTTPS->HTTP).
    /// </summary>
    StrictOriginWhenCrossOrigin,

    /// <summary>
    /// Send a full URL (stripped from parameters) when performing a a same-origin or cross-origin request.
    /// </summary>
    UnsafeUrl,
}
