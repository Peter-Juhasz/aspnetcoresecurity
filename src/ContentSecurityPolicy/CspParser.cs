using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PeterJuhasz.AspNetCore.Extensions.Security
{
    /// <summary>
    /// A Content Security Policy restricts which resources a web page can load, protecting against risks such as cross-site scripting.
    /// </summary>
    public class CspParser : ICspParser
    {
        private readonly Dictionary<string, IList<string>> _parsedPolicy = new Dictionary<string, IList<string>>();

        /// <summary>
        /// Parses a Content Security Policy HTTP header value as an instance of <see cref="CspOptions"/>.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public CspOptions ParsePolicy(string policy)
        {
            var directives = policy.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string directive in directives)
            {
                ParseDirective(directive);
            }

            return ConvertParsedPolicyToCspOptions();
        }

        private CspOptions ConvertParsedPolicyToCspOptions()
        {
            var options = new CspOptions();

            options.BaseUri = ConvertParsedDirectiveToCspDirective("base-uri");
            options.BlockAllMixedContent = ConvertParsedDirectiveToBool("block-all-mixed-content");
            options.ChildSrc = ConvertParsedDirectiveToCspDirective("child-src");
            options.ConnectSrc = ConvertParsedDirectiveToCspDirective("connect-src");
            options.DefaultSrc = ConvertParsedDirectiveToCspDirective("default-src");
            options.FontSrc = ConvertParsedDirectiveToCspDirective("font-src");
            options.FormAction = ConvertParsedDirectiveToCspDirective("form-action");
            options.FrameAncestors = ConvertParsedDirectiveToCspDirective("frame-ancestors");
            options.FrameSrc = ConvertParsedDirectiveToCspDirective("frame-src");
            options.ImgSrc = ConvertParsedDirectiveToCspDirective("img-src");
            options.ManifestSrc = ConvertParsedDirectiveToCspDirective("manifest-src");
            options.MediaSrc = ConvertParsedDirectiveToCspDirective("media-src");
            options.NavigationTo = ConvertParsedDirectiveToCspDirective("navigation-to");
            options.ObjectSrc = ConvertParsedDirectiveToCspDirective("object-src");
            options.PluginTypes = ConvertParsedDirectiveToCollection("plugin-types");
            options.ReflectedXss = ConvertParsedDirectiveToEnum<CspReflectedXss>("reflected-xss");
            options.ReportUri = ConvertParsedDirectiveToUri("report-uri");
            options.RequireSriFor = ConvertParsedDirectiveToEnum<CspRequireSRIResources>("require-sri-for");
            options.Sandbox = ConvertParsedDirectiveToEnum<CspSandboxRules>("sandbox");
            options.ScriptSrc = ConvertParsedDirectiveToScriptDirective("script-src");
            options.StyleSrc = ConvertParsedDirectiveToStyleDirective("style-src");
            options.UpgradeInsecureRequests = ConvertParsedDirectiveToBool("upgrade-insecure-requests");
            options.WorkerSrc = ConvertParsedDirectiveToCspDirective("worker-src");

            return options;
        }

        private StyleCspDirective ConvertParsedDirectiveToStyleDirective(string directiveName)
        {
            if (_parsedPolicy.ContainsKey(directiveName))
            {
                var directive = new StyleCspDirective();
                foreach (var source in _parsedPolicy[directiveName])
                {
                    directive = directive.AddSource(source);
                }
                return directive;
            }
            return null;
        }

        private ScriptCspDirective ConvertParsedDirectiveToScriptDirective(string directiveName)
        {
            if (_parsedPolicy.ContainsKey(directiveName))
            {
                var directive = new ScriptCspDirective();
                foreach (var source in _parsedPolicy[directiveName])
                {
                    directive = directive.AddSource(source);
                }
                return directive;
            }
            return null;
        }

        private bool ConvertParsedDirectiveToBool(string directiveName)
        {
            return _parsedPolicy.ContainsKey(directiveName);
        }

        private Uri ConvertParsedDirectiveToUri(string directiveName)
        {
            if (_parsedPolicy.ContainsKey(directiveName) && _parsedPolicy[directiveName].Count > 0)
            {
                return new Uri(_parsedPolicy[directiveName][0], UriKind.RelativeOrAbsolute);
            }
            return null;
        }

        private T? ConvertParsedDirectiveToEnum<T>(string directiveName) where T : struct
        {
            if (_parsedPolicy.ContainsKey(directiveName) && _parsedPolicy[directiveName].Count > 0)
            {
                T parseResult;
                var valueToParse = string.Join(",", _parsedPolicy[directiveName].ToArray());
                valueToParse = valueToParse.Replace("-", string.Empty); 

                if (Enum.TryParse<T>(valueToParse, true, out parseResult))
                {
                    return parseResult;
                }
            }
            return null;
        }

        private IReadOnlyCollection<string> ConvertParsedDirectiveToCollection(string directiveName)
        {
            if (_parsedPolicy.ContainsKey(directiveName))
            {
                return new ReadOnlyCollection<string>(_parsedPolicy[directiveName]);
            }
            return new ReadOnlyCollection<string>(new List<string>());
        }

        private CspDirective ConvertParsedDirectiveToCspDirective(string directiveName)
        {
            if (_parsedPolicy.ContainsKey(directiveName))
            {
                var directive = new CspDirective();
                foreach (var source in _parsedPolicy[directiveName])
                {
                   directive = directive.AddSource(source);
                }
                return directive;
            }
            return null;
        }

        private void ParseDirective(string directive)
        {
            var splitDirective = directive.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitDirective.Length == 1)
            {
                var directiveType = splitDirective[0].ToLowerInvariant();
                if (!_parsedPolicy.ContainsKey(directiveType)) _parsedPolicy.Add(directiveType, new List<string>());
            }
            if (splitDirective.Length > 1)
            {
                var sources = new List<string>(splitDirective);
                var directiveType = sources[0].ToLowerInvariant();
                sources.RemoveAt(0);

                ParseSources(directiveType, sources);
            }
        }

        private void ParseSources(string directiveType, IEnumerable<string> sources)
        {
            if (!_parsedPolicy.ContainsKey(directiveType)) _parsedPolicy.Add(directiveType, new List<string>());

            foreach (string source in sources)
            {
                if (!_parsedPolicy[directiveType].Contains(source)) _parsedPolicy[directiveType].Add(source);
            }
        }
    }
}
