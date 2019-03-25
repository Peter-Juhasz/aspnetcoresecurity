# ASP.NET Core Security Extensions

```
dotnet add package PeterJuhasz.AspNetCore.Security.Extensions
```

Contains a set of extensions which help you make your web applications more secure.

Note: for ASP.NET Core 1.0 use package version `1.0.0`, otherwise for ASP.NET Core 2.0 use `2.0.0`.


## Features

### Content-Security-Policy

Adds the `Content-Security-Policy`, `X-Content-Security-Policy` and `X-Webkit-CSP` headers to responses with content type `text/html`.

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

Parse an existing `Content-Security-Policy` header value:

```csharp
CspParser parser = new CspParser();
CspOptions parsedPolicy = parser.ParsePolicy(Context.Response.Headers["Content-Security-Policy"]);
```

### Expect-CT

Adds the `Expect-CT` header which allows sites to opt in to reporting and/or enforcement of Certificate Transparency requirements.

```csharp
app.UseExpectCT(enforce: true, maxAge: TimeSpan.FromHours(1));
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

Adds the `Strict-Transport-Security` header to all responses.

```csharp
app.UseHttpStrictTransportSecurity();
```

### HTTP Public Key Pinning

Adds the `Public-Key-Pinning` header to all responses.

```csharp
app.UseHttpPublicKeyPinning(options => options
    .Pin(fingerprint1, HttpPublicKeyPinningHashAlgorithm.Sha256)
    .Pin(fingerprint2, HttpPublicKeyPinningHashAlgorithm.Sha256)
);
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