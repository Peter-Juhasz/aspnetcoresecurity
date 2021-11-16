using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder;

public record class RedirectPolicyOptions(IReadOnlyCollection<Uri> AllowedBaseUris);
