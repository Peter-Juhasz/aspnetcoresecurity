using System;

namespace Microsoft.AspNetCore.Builder;

public static class PermissionsPolicyPermissions
{
    public const string Accelerometer = "accelerometer";
    public const string AmbientLightSensor = "ambient-light-sensor";
    public const string Autoplay = "autoplay";
    public const string Battery = "battery";
    public const string BackgroundFetch = "background-fetch";
    public const string BackgroundSync = "background-sync";
    public const string Bluetooth = "bluetooth";
    public const string Camera = "camera";
    public const string ClipboardRead = "clipboard-read";
    public const string ClipboardWrite = "clipboard-write";
    public const string DeviceInfo = "device-info";
    public const string DisplayCapture = "display-capture";
    public const string DocumentDomain = "document-domain";
    public const string EncryptedMedia = "encrypted-media";
    public const string Fullscreen = "fullscreen";
    public const string Geolocation = "geolocation";
    public const string Gyroscope = "gyroscope";
    public const string LayoutAnimations = "layout-animations";
    public const string LegacyImageFormats = "legacy-image-formats";
    public const string Magnetometer = "magnetometer";
    public const string Microphone = "microphone";
    public const string Midi = "midi";
    public const string Nfc = "nfc";
    public const string Notifications = "notifications";
    public const string OversizedImages = "oversized-images";
    public const string Payment = "payment";
    public const string PersistentStorage = "persistent-storage";
    public const string PictureInPicture = "picture-in-picture";
    public const string PublickeyCredentialsGet = "publickey-credentials-get";
    public const string Speaker = "speaker";
    public const string SpeakerSelection = "speaker-selection";
    public const string SyncXhr = "sync-xhr";
    public const string ScreenWakeLock = "screen-wake-lock";
    public const string UnoptimizedImages = "unoptimized-images";
    public const string UnsizedMedia = "unsized-media";
    public const string Usb = "usb";
    [Obsolete]
    public const string Vibrate = "vibrate";
    [Obsolete]
    public const string Vr = "vr";
    public const string WakeLock = "wake-lock";
    [Obsolete]
    public const string Webauthn = "webauthn";
    public const string XrSpatialTracking = "xr-spatial-tracking";
}
