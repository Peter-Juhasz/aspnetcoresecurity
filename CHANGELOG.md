## 5.0.0
New features:
 - **.NET 5** and ASP.NET Core 5.0+ support
 - **Performance improvements**
   - All middlewares are strongly typed using the [`IMiddleware`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.imiddleware) interface
   - All middlewares are stateless, so they don't have to be instantiated for each and every request
   - Response headers are not parsed into objects, low level APIs are satisfactory while providing the same security
   - Header values are pre-rendered, so middlewares are basically allocation free from now (instead of constructing them on the fly, which resulted in lots of new `String` objects on the heap). *Note: this also means that configuration changes won't take effect while running.*

Deprecation:
 - **Frame-Options**: `allow-from`
 - **Frame-Options**: use Content Security Policy instead

## 3.1.0
 - **Permissions-Policy**: support for [Permissions Policy](https://w3c.github.io/webappsec-permissions-policy/)
 - **Feature-Policy**: marked as obsolete

## 3.0.2
 - Fix: **Require Authenticated Identity**: middleware didn't interrupt the pipeline.

## 3.0.1
 - **Report-To**: Added support for `Report-To` header
 - **Upgrade Insecure Resources**: Tag helper to upgrade insecure resource references (like scripts, styles, images, links) to `https://`
 - Fix: **Subresource Integrity**: increased its order, so it is the last to be executed

# 3.0.0
New features:
 - ASP.NET Core 3.0+ support
 - Nullable reference types support
 - **Subresource Integrity**: Tag helper to add `integrity` attribute to remote resources
 - **Feature-Policy**: Added support
 - **NoOpener**: Tag helper to add missing `noopener` relationships to `a` tags
 - **Require Authenticated Identity**: Middleware that requires an authenticated identity on the HttpContext to proceed
 - **CSP**: Added support for `prefetch-src`
 - **CSP**: Added support for `script-src-attr`
 - **CSP**: Added support for `script-src-elem`
 - **CSP**: Added support for `style-src-attr`
 - **CSP**: Added support for `style-src-elem`
 - **X-Robots-Tag**: Added support for specifying user agent
 - **X-Robots-Tag**: Added support for `max-snippet`
 - **X-Robots-Tag**: Added support for `max-image-preview`
 - **X-Robots-Tag**: Added support for `max-video-preview`

Breaking changes:
 - `CspOptions` was renamed to `CspDirectiveList`
 - `FrameOptionOptions` was renamed to `FrameOptionsDirective`
 - `XRobotsTagHeaderValue` was renamed to `RobotsTagDirectiveList`

Deprecation:
 - **CSP**: Removed support for `reflected-xss`
 - **CSP**: Removed support for `X-WebKit-CSP` and `X-Content-Security-Policy` header names as CSP is widely supported now
 - **HTTP Strict Transport Security**: Removed support as it has built-in support in ASP.NET Core 3.0
 - **HTTP Public Key Pinning**: Marked as obsolete, as it has been deprecated by Chrome/IE/Edge
 - **X-XSS-Protection**: Marked as obsolete, as it has been permanently disabled by Chrome/Edge and has never been implemented in Firefox

Fixes:
 - **CSP**: Fixed crash on developer exception page when `style-src` or `script-src` was not set


## 2.1.0
- **Expect-CT**: support for Certificate Transparency
- **CSP**: Add source to directives by hash
- **CSP**: Add source to directives in hash form by content
- **CSP**: `reflected-xss` directive is turned off by default due to lack of browser support

## 2.0.1
- **CSP** Fixed: The directive name `font-src` was accidentally re-used for the `form-action` directive [#1](https://github.com/Peter-Juhasz/aspnetcoresecurity/pull/1) [@BlueNinjaSoftware](https://github.com/BlueNinjaSoftware)

# 2.0.0
- ASP.NET Core 2+ support

# 1.0.0
- ASP.NET Core 1.0-2.0 support
- Content-Security-Policy
- Frame-Options
- HTTP Public Key Pinning
- HTTP Strict Transport Security
- Redirect Policy
- Referrer Policy
- X-Content-Type-Options
- X-Download-Options
- X-Permitted-Cross-Domain-Policies
- X-Robots-Tag
- X-UA-Compatible
- X-XSS-Protection
