namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Represents a feature policy directive.
/// </summary>
/// <param name="Directive">The feature.</param>
/// <param name="AllowList">A feature policy allowlist is conceptually a set of origins.</param>
public record class FeatureDirective(PolicyFeature Directive, string AllowList);
