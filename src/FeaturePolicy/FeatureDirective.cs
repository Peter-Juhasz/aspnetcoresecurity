namespace Microsoft.AspNetCore.Builder
{
    public class FeatureDirective
    {
        public FeatureDirective(PolicyFeature directive, string allowList)
        {
            Directive = directive;
            AllowList = allowList;
        }

        /// <summary>
        /// 
        /// </summary>
        public PolicyFeature Directive { get; }

        /// <summary>
        /// A feature policy allowlist is conceptually a set of origins.
        /// </summary>
        public string AllowList { get; }
    }
}
