using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    public class ReportingGroup
    {
        public ReportingGroup(string? name, TimeSpan maxAge, IReadOnlyCollection<ReportingEndpoint> endpoints, bool includeSubdomains = false)
        {
            Name = name;
            MaxAge = maxAge;
            Endpoints = endpoints.ToList();
            IncludeSubdomains = includeSubdomains;
        }
        public ReportingGroup(TimeSpan maxAge, IReadOnlyCollection<ReportingEndpoint> endpoints)
            : this(null, maxAge, endpoints)
        { }
        public ReportingGroup(TimeSpan maxAge, ReportingEndpoint endpoint)
            : this(maxAge, new[] { endpoint })
        { }
        public ReportingGroup(TimeSpan maxAge, string endpoint)
            : this(maxAge, new ReportingEndpoint(endpoint))
        { }

        public string? Name { get; set; }

        public TimeSpan MaxAge { get; set; }

        public ICollection<ReportingEndpoint> Endpoints { get; }

        public bool IncludeSubdomains { get; set; }

        public override string ToString()
        {
            using var buffer = new MemoryStream(capacity: Endpoints.Count * 128);
            using var writer = new Utf8JsonWriter(buffer);

            writer.WriteStartObject();
            if (Name != null)
            {
                writer.WriteString("name", Name);
            }
            writer.WriteNumber("max_age", (int)MaxAge.TotalSeconds);
            if (IncludeSubdomains == true)
            {
                writer.WriteBoolean("include_subdomains", IncludeSubdomains);
            }
            writer.WritePropertyName("endpoints");
            writer.WriteStartArray();

            if (Endpoints.All(e => e.Priority == default && e.Weight == default))
            {
                foreach (var endpoint in Endpoints)
                {
                    writer.WriteStringValue(endpoint.Uri.ToString());
                }
            }
            else
            {
                foreach (var endpoint in Endpoints)
                {
                    writer.WriteStartObject();
                    writer.WriteString("url", endpoint.Uri.ToString());
                    if (endpoint.Priority != null)
                    {
                        writer.WriteNumber("priority", endpoint.Priority.Value);
                    }
                    if (endpoint.Weight != null)
                    {
                        writer.WriteNumber("weight", endpoint.Weight.Value);
                    }
                    writer.WriteEndObject();
                }
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Flush();

            return Encoding.UTF8.GetString(buffer.ToArray());
        }
    }
}
