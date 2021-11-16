using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Builder;

public class PermissionsPolicyDirectiveList : IEnumerable<KeyValuePair<string, ISet<string>>>
{
    public PermissionsPolicyDirectiveList()
        : this(new Dictionary<string, ISet<string>>())
    { }
    internal PermissionsPolicyDirectiveList(IDictionary<string, ISet<string>> items)
    {
        Items = items;
    }

    protected IDictionary<string, ISet<string>> Items { get; }


    protected PermissionsPolicyDirectiveList AddCore(string feature, string value)
    {
        ISet<string>? allowList = EnsureAllowList(feature);

        allowList.Add(value);

        return this;
    }

    internal ISet<string> EnsureAllowList(string feature)
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
        allowList.Add(PermissionsPolicyTokens.All);
        return this;
    }

    /// <summary>
    /// The feature is allowed by default in top-level browsing contexts and in nested browsing contexts (iframes) in the same origin. The feature is not allowed in cross-origin documents in nested browsing contexts.
    /// </summary>
    /// <param name="feature"></param>
    /// <returns></returns>
    public PermissionsPolicyDirectiveList AddSelf(string feature) => AddCore(feature, PermissionsPolicyTokens.Self);

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


    public PermissionsPolicyDirectiveList Copy() => new PermissionsPolicyDirectiveList(new Dictionary<string, ISet<string>>(Items));

    internal PermissionsPolicyDirectiveList Merge(IList<Change> changes)
    {
        var copy = Copy();

        foreach (var change in changes)
        {
            switch (change.Operation)
            {
                case ChangeOperation.Allow:
                    switch (change.Value)
                    {
                        case PermissionsPolicyTokens.All:
                            copy.AddAll(change.Feature);
                            break;

                        default:
                            copy.Add(change.Feature, change.Value);
                            break;
                    }
                    break;

                case ChangeOperation.Disallow:
                    var list = copy.EnsureAllowList(change.Feature);
                    switch (change.Value)
                    {
                        case PermissionsPolicyTokens.All:
                            list.Clear();
                            break;

                        default:
                            if (!list.Remove(change.Value))
                            {
                                throw new InvalidOperationException($"Value '{change.Value}' can't be removed for feature '{change.Feature}', because it has never been added.");
                            }
                            break;
                    }
                    break;
            }
        }

        return copy;
    }


    public override string ToString()
    {
        return string.Join(", ", this.Select(i => $"{i.Key}=({String.Join(' ', i.Value)})"));
    }


    public IEnumerator<KeyValuePair<string, ISet<string>>> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
