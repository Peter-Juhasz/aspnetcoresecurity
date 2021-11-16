namespace PeterJuhasz.AspNetCore.Extensions.Security;

public enum PermittedCrossDomainPolicy
{
    /// <summary>
    /// No policy files are allowed anywhere on the target server, including this master policy file. 
    /// </summary>
    None = 0,

    /// <summary>
    /// Only this master policy file is allowed. 
    /// </summary>
    MasterOnly,

    /// <summary>
    /// Only policy files served with Content-Type: text/x-cross-domain-policy are allowed. 
    /// </summary>
    ByContentType,

    /// <summary>
    /// Only policy files whose file names are crossdomain.xml (i.e. URLs ending in /crossdomain.xml) are allowed.
    /// </summary>
    ByFtpFileName,

    /// <summary>
    /// All policy files on this target domain are allowed. 
    /// </summary>
    All,
}
