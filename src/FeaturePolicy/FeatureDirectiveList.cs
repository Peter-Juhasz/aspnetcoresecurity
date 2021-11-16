using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Builder;

public class FeatureDirectiveList : IEnumerable<FeatureDirective>
{
    protected IList<FeatureDirective> Items { get; } = new List<FeatureDirective>();

    private static readonly IReadOnlyDictionary<PolicyFeature, string> FeatureNames = new Dictionary<PolicyFeature, string>
        {
             { PolicyFeature.Accelerometer, "accelerometer" },
             { PolicyFeature.AmbientLightSensor, "ambient-light-sensor" },
             { PolicyFeature.Autoplay, "autoplay" },
             { PolicyFeature.Camera, "camera" },
             { PolicyFeature.DisplayCapture, "display-capture" },
             { PolicyFeature.DocumentDomain, "document-domain" },
             { PolicyFeature.EncryptedMedia, "encrypted-media" },
             { PolicyFeature.Fullscreen, "fullscreen" },
             { PolicyFeature.Geolocation, "geolocation" },
             { PolicyFeature.Gyroscope, "gyroscope" },
             { PolicyFeature.LayoutAnimations, "layout-animations" },
             { PolicyFeature.LegacyImageFormats, "legacy-image-formats" },
             { PolicyFeature.Magnetometer, "magnetometer" },
             { PolicyFeature.Microphone, "microphone" },
             { PolicyFeature.Midi, "midi" },
             { PolicyFeature.OversizedImages, "oversized-images" },
             { PolicyFeature.Payment, "payment" },
             { PolicyFeature.PictureInPicture, "picture-in-picture" },
             { PolicyFeature.Speaker, "speaker" },
             { PolicyFeature.SyncXhr, "sync-xhr" },
             { PolicyFeature.UnoptimizedImages, "unoptimized-images" },
             { PolicyFeature.UnsizedMedia, "unsized-media" },
             { PolicyFeature.Usb, "usb" },
             { PolicyFeature.Vibrate, "vibrate" },
             { PolicyFeature.Vr, "vr" },
             { PolicyFeature.WakeLock, "wake-lock" },
             { PolicyFeature.Webauthn, "webauthn" },
             { PolicyFeature.XrSpatialTracking, "xp-spatial-tracking" },
        };


    protected FeatureDirectiveList AddCore(PolicyFeature directive, string value)
    {
        Items.Add(new FeatureDirective(directive, value));
        return this;
    }

    /// <summary>
    /// The feature is allowed for specific origins (for example, https://example.com). Origins should be separated by a space.
    /// </summary>
    /// <param name="directive"></param>
    /// <param name="origin">The feature is allowed for specific origins (for example, https://example.com). Origins should be separated by a space.</param>
    /// <returns></returns>
    public FeatureDirectiveList Add(PolicyFeature directive, string origin) => AddCore(directive, origin);

    /// <summary>
    /// The feature is allowed for specific origins (for example, https://example.com). Origins should be separated by a space.
    /// </summary>
    /// <param name="directive"></param>
    /// <param name="origins"></param>
    /// <returns></returns>
    public FeatureDirectiveList Add(PolicyFeature directive, IEnumerable<string> origins) => AddCore(directive, string.Join(' ', origins));

    /// <summary>
    /// The feature is allowed by default in top-level browsing contexts and all nested browsing contexts (iframes).
    /// </summary>
    /// <param name="directive"></param>
    /// <returns></returns>
    public FeatureDirectiveList AddAll(PolicyFeature directive) => AddCore(directive, "*");

    /// <summary>
    /// The feature is allowed by default in top-level browsing contexts and in nested browsing contexts (iframes) in the same origin. The feature is not allowed in cross-origin documents in nested browsing contexts.
    /// </summary>
    /// <param name="directive"></param>
    /// <returns></returns>
    public FeatureDirectiveList AddSelf(PolicyFeature directive) => AddCore(directive, "'self'");

    /// <summary>
    /// The feature is disabled in top-level and nested browsing contexts.
    /// </summary>
    /// <param name="directive"></param>
    /// <returns></returns>
    public FeatureDirectiveList AddNone(PolicyFeature directive) => AddCore(directive, "'none'");

    /// <summary>
    /// The feature will be allowed in this document, and in all nested browsing contexts (iframes) in the same origin.
    /// </summary>
    /// <param name="directive"></param>
    /// <returns></returns>
    public FeatureDirectiveList AddSrc(PolicyFeature directive) => AddCore(directive, "'src'");


    public override string ToString()
    {
        return string.Join("; ", this.Select(i => $"{FeatureNames[i.Directive]} {i.AllowList}"));
    }


    public IEnumerator<FeatureDirective> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
