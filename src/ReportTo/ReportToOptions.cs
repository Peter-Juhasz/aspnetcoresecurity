
using PeterJuhasz.AspNetCore.Extensions.Security;

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder;

public record class ReportToOptions(IReadOnlyList<ReportingGroup> Groups);
