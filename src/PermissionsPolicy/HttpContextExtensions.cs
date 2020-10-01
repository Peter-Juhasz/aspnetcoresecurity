using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static void AllowPermissionsPolicy(this HttpContext context, string feature, string value)
        {
            context.EnsureChanges().Add(new Change(ChangeOperation.Allow, feature, value));
        }

        public static void DisallowPermissionsPolicy(this HttpContext context, string feature, string value)
        {
            context.EnsureChanges().Add(new Change(ChangeOperation.Disallow, feature, value));
        }

        internal static IList<Change> EnsureChanges(this HttpContext context)
        {
            if (context.Items.TryGetValue(nameof(PermissionsPolicyDirectiveList), out var changes))
            {
                return (IList<Change>)changes!;
            }

            var list = new List<Change>();
            context.Items.Add(nameof(PermissionsPolicyDirectiveList), list);
            return list;
        }
    }
}
