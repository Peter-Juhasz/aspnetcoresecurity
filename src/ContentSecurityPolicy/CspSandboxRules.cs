using System;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    [Flags]
    public enum CspSandboxRules
    {
        Sandbox = 0,

        AllowForms,
        AllowSameOrigin,
        AllowScripts,
        AllowPopups,
        AllowModals,
        AllowOrientationLock,
        AllowPointerLock,
        AllowPresentation,
        AllowPopupsToEscapeSandbox,
        AllowTopNavigation,
    }
}
