namespace Microsoft.AspNetCore.Http;

internal sealed record Change(ChangeOperation Operation, string Feature, string Value);
