# ASP.NET Core Security Extensions

```
dotnet add package PeterJuhasz.AspNetCore.Security.Extensions
```

Contains a set of extensions which can help you make your web applications more secure. You can also install each feature invididually as a separate package.


## Features

### Content-Security-Policy

Adds the `Content-Security-Policy` headers to responses with content type `text/html`.

```csharp
app.UseContentSecurityPolicy(new CspOptions
{
    DefaultSrc = CspDirective.None,
    StyleSrc = StyleCspDirective.Self,
    ScriptSrc = ScriptCspDirective.Self
        .AddSource(new Uri("https://az416426.vo.msecnd.net/")), // Application Insights
    ImgSrc = CspDirective.Self
        .AddDataScheme(),
    FontSrc = CspDirective.Self,
    ConnectSrc = CspDirective.Empty
        .AddSource(new Uri("https://dc.services.visualstudio.com/")),
});
```

### Cross Origin Resource Sharing

Use the built-in support in ASP.NET Core 3.0.

### Expect-CT

Adds the `Expect-CT` header which allows sites to opt in to reporting and/or enforcement of Certificate Transparency requirements.

```csharp
app.UseExpectCT(enforce: true, maxAge: TimeSpan.FromHours(1));
```

### Feature-Policy

Adds the `Feature-Policy` header to responses with content type `text/html`.

```csharp
app.UseFeaturePolicy(
    new FeatureDirectiveList()
        .Add(FeatureDirective.Payment, "https://payment.example.org/")
        .AddNone(FeatureDirective.Microphone)
        .AddSelf(FeatureDirective.FullScreen)
);
```

### Frame Options

Adds the `Frame-Options` and `X-Frame-Options` headers to responses with content type `text/html`.

```csharp
app.UseFrameOptions(FrameOptionsPolicy.Deny);
```

If you want to enable displaying the page in a frame on a particular origin, you can set it like this:

```csharp
app.UseFrameOptions(new Uri("https://www.example.org"));
```

### HTTP Strict Transport Security

Use the built-in support in ASP.NET Core 3.0.

### HTTP Public Key Pinning

Adds the `Public-Key-Pinning` header to all responses.

```csharp
app.UseHttpPublicKeyPinning(options => options
    .Pin(fingerprint1, HttpPublicKeyPinningHashAlgorithm.Sha256)
    .Pin(fingerprint2, HttpPublicKeyPinningHashAlgorithm.Sha256)
);
```

### NoOpener

A tag helper that adds the missing `noopener` link relationship type to your `a` tags that open in another frame and doesn't reference the same origin.

Add an import for the tag helper (in your `_ViewImports.cshtml` if you have one):
```cshtml
@addTagHelper *, PeterJuhasz.AspNetCore.Extensions.Security.NoOpener
```

You don't need any additional changes, the tag helper applies to all links, for example:
```html
<a href="https://example.org/malicious.html" target="_blank">Click here</a>
```

And adds the missing `rel` attribute:
```html
<a href="https://example.org/malicious.html" target="_blank" rel="noopener">Click here</a>
```

### Redirect Policy

Restricts server-side redirects only to trusted origins.

```csharp
app.UseRedirectPolicy();
```

You can also specify the trusted origins:

```csharp
app.UseRedirectPolicy(allowedBaseUris: "https://www.example.org");
```

### Referrer Policy

Adds the `Referrer-Policy` header to all responses.

```csharp
app.UseReferrerPolicy(ReferrerPolicy.SameOrigin);
```

### Report-To
Add the `Report-To` header to all responses.

```csharp
app.UseReportTo(new ReportingGroup(
    maxAge: TimeSpan.FromDays(30),
    endpoint: "https://example.org/browser-report"
));
```

### Require Authenticated Identity
This is a middleware that you can use to require an authenticated identity on the `HttpContext` to proceed. For example, you can use this middleware to require authentication for static files.

```csharp
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/dist"),
    branch => branch.UseRequireAuthenticatedIdentity()
);
```

Notes:
 - `401` is returned in case of no authenticated user

### Subresource Integrity
A tag helper that computes the `integrity` attribute for linked styles and scripts from remote origins. It also adds the `crossorigin` attribute with `anonymous` value.

Add the required services (in your `Startup.cs`):
```cs
services.AddSubresourceIntegrity();
```

Add an import for the tag helper (in your `_ViewImports.cshtml` if you have one):
```cshtml
@addTagHelper *, PeterJuhasz.AspNetCore.Extensions.Security.SubresourceIntegrity
```

You don't need any additional changes, the tag helper applies to styles and scripts, for example:
```html
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
```

And adds the `integrity` and `crossorigin` attributes:
```html
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha256-YLGeXaapI0/5IgZopewRJcFXomhRMlYYjugPLSyNjTY=" crossorigin="anonymous" />
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha256-CjSoeELFOcH0/uxWu6mC/Vlrc1AARqbm/jiiImDGV3s=" crossorigin="anonymous"></script>
```

Notes:
 - If the `integrity` attribute is already included, it skips that element and doesn't compute and validate it.
 - In case the remote resource is not available, a warning is logged and the integrity attribute is not included. Page rendering is not interrupted.
 - The hash algorithm used is SHA-256.
 - Hashes are cached in a memory cache indefinitely.

### Upgrade Insecure Resources
A tag helper that upgrades insecure links, style, script and image references to HTTPS.

Add an import for the tag helper (in your `_ViewImports.cshtml` if you have one):
```cshtml
@addTagHelper *, PeterJuhasz.AspNetCore.Extensions.Security.UpgradeInscureResources
```

You don't need any additional changes, the tag helper applies to all `href` and `src` attributes:
```html
<a href="http://example.org/page">Click here</a>
<script src="http://example.org/script.js"></script>
```

Will be rewritten to:
```html
<a href="https://example.org/page">Click here</a>
<script src="https://example.org/script.js"></script>
```

### X-Content-Type-Options

Adds the `X-Content-Type-Options` header to all responses.

```csharp
app.UseXContentTypeOptions(XContentTypeOptions.NoSniff);
```

### X-Download-Options

Adds the `X-Download-Options` header to each file download.

```csharp
app.UseXDownloadOptions(XDownloadOptions.NoOpen);
```

### X-Permitted-Cross-Domain-Policies

Adds `X-Permitted-Cross-Domain-Policies` header to all responses.

```csharp
app.UseXPermittedCrossDomainPolicies(PermittedCrossDomainPolicy.None);
```

### X-Robots-Tag

Adds the `X-Robots-Tag` header to all responses.

```csharp
app.UseXRobotsTag(noIndex: true, noFollow: true);
```

### X-UA-Compatible

Adds the `X-UA-Compatible` header to each response with `text/html` media type.

```csharp
app.UseXUACompatible(InternetExplorerCompatibiltyMode.Edge);
```

### X-XSS-Protection

Adds the `X-XSS-Protection` header to each response with `text/html` media type. The default setting enables protection and sets it to `block` mode.

```csharp
app.UseXXSSProtection();
```