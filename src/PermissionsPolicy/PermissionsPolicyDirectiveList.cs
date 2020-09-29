using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public class PermissionsPolicyDirectiveList : IEnumerable<KeyValuePair<string, ISet<string>>>
    {
        protected IDictionary<string, ISet<string>> Items { get; } = new Dictionary<string, ISet<string>>();


        protected PermissionsPolicyDirectiveList AddCore(string feature, string value)
        {
            ISet<string>? allowList = EnsureAllowList(feature);

            allowList.Add(value);

            return this;
        }

        protected ISet<string> EnsureAllowList(string feature)
        {
            if (!Items.TryGetValue(feature, out var allowList))
            {
                allowList = new HashSet<string>();
                Items.Add(feature, allowList);
            }

            return allowList;
        }

        /// <summary>
        /// The feature is allowed for specific origins (for example, https://example.com). Origins should be separated by a space.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="origin">The feature is allowed for specific origins (for example, https://example.com). Origins should be separated by a space.</param>
        /// <returns></returns>
        public PermissionsPolicyDirectiveList Add(string feature, string origin) => AddCore(feature, origin);

        /// <summary>
        /// The feature is allowed by default in top-level browsing contexts and all nested browsing contexts (iframes).
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public PermissionsPolicyDirectiveList AddAll(string feature)
        {
            var allowList = EnsureAllowList(feature);
            if (allowList.Any())
            {
                allowList.Clear();
            }
            allowList.Add("*");
            return this;
        }

        /// <summary>
        /// The feature is allowed by default in top-level browsing contexts and in nested browsing contexts (iframes) in the same origin. The feature is not allowed in cross-origin documents in nested browsing contexts.
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public PermissionsPolicyDirectiveList AddSelf(string feature) => AddCore(feature, "self");

        /// <summary>
        /// The feature is disabled in top-level and nested browsing contexts.
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public PermissionsPolicyDirectiveList AddNone(string feature)
        {
            var allowList = EnsureAllowList(feature);
            if (allowList.Any())
            {
                throw new InvalidOperationException("The allow list can not be set to none, because it already contains values.");
            }
            return this;
        }


        public override string ToString()
        {
            return string.Join(", ", this.Select(i => $"{i.Key}=({String.Join(' ', i.Value)})"));
        }


        public IEnumerator<KeyValuePair<string, ISet<string>>> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
