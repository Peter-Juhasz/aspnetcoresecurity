using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace PeterJuhasz.AspNetCore.Extensions.Security;

/// <summary>
/// Represents a reporting group of the Report-To header.
/// </summary>
/// <param name="Name">The name of the group.</param>
/// <param name="MaxAge">The time the browser should remember the group.</param>
/// <param name="Endpoints">The endpoints of the group.</param>
/// <param name="IncludeSubdomains">Whether the group applies to subdomains as well.</param>
public record class ReportingGroup(TimeSpan MaxAge, IReadOnlyCollection<ReportingEndpoint> Endpoints, string? Name = null, bool IncludeSubdomains = false)
{
    public ReportingGroup(TimeSpan maxAge, ReportingEndpoint endpoint)
        : this(maxAge, [endpoint])
    { }

    public ReportingGroup(TimeSpan maxAge, string endpoint)
        : this(maxAge, new ReportingEndpoint(endpoint))
    { }

    public override string ToString()
    {
        using var buffer = new MemoryStream(capacity: Endpoints.Count * 128);
        using var writer = new Utf8JsonWriter(buffer);

        writer.WriteStartObject();
        if (Name is not null)
            writer.WriteString("name", Name);

        writer.WriteNumber("max_age", (int)MaxAge.TotalSeconds);
        if (IncludeSubdomains)
            writer.WriteBoolean("include_subdomains", IncludeSubdomains);

        writer.WritePropertyName("endpoints");
        writer.WriteStartArray();

        if (Endpoints.All(e => e.Priority is null && e.Weight is null))
        {
            foreach (var endpoint in Endpoints)
                writer.WriteStringValue(endpoint.Uri.ToString());
        }
        else
        {
            foreach (var endpoint in Endpoints)
            {
                writer.WriteStartObject();
                writer.WriteString("url", endpoint.Uri.ToString());
                if (endpoint.Priority is { } priority)
                    writer.WriteNumber("priority", priority);
                if (endpoint.Weight is { } weight)
                    writer.WriteNumber("weight", weight);
                writer.WriteEndObject();
            }
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();

        return Encoding.UTF8.GetString(buffer.ToArray());
    }
}
