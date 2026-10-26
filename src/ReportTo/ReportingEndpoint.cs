using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents an endpoint of a reporting group.
/// </summary>
/// <param name="Uri">The URI reports are sent to.</param>
/// <param name="Priority">The priority of the endpoint, lower values are tried first.</param>
/// <param name="Weight">The weight of the endpoint among endpoints with the same priority.</param>
public record class ReportingEndpoint(Uri Uri, int? Priority = null, int? Weight = null)
{
    public ReportingEndpoint(string uri)
        : this(Uri: new Uri(uri, UriKind.Absolute))
    { }

    public int? Priority { get; init; } = Priority is < 0
        ? throw new ArgumentOutOfRangeException(nameof(Priority))
        : Priority;
}
