using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

public class ReportingEndpoint
{
    public ReportingEndpoint(Uri uri)
    {
        Uri = uri;
    }
    public ReportingEndpoint(string uri)
        : this(new Uri(uri, UriKind.Absolute))
    { }
    public ReportingEndpoint(Uri uri, int priority)
        : this(uri)
    {
        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority));
        }

        Priority = priority;
    }

    public Uri Uri { get; set; }

    public int? Priority { get; set; }

    public int? Weight { get; set; }
}
