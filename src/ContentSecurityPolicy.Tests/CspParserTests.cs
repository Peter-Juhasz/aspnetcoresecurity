using System;
using PeterJuhasz.AspNetCore.Extensions.Security;
using Xunit;

namespace ContentSecurityPolicy.Tests
{
    public class CspParserTests
    {
        [Fact]
        public void BaseUriIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "base-uri https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void BlockAllMixedContentIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "block-all-mixed-content";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.True(parsedPolicy.BlockAllMixedContent);
        }

        [Fact]
        public void ChildSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "child-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void ConnectSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "connect-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void DefaultSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "default-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void FontSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "font-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void FormActionIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "form-action https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void FrameAncestorsIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "frame-ancestors https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void FrameSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "frame-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void ImgSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "img-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void ManifestSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "manifest-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void MediaSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "media-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void NavigationToIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "navigation-to https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void ObjectSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "object-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void PluginTypesIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "plugin-types application/x-java-applet";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void ReflectedXssIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "reflected-xss allow";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void ReportUriIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "report-uri https://example.org/";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void RequireSriForIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "require-sri-for style script";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void SandboxIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "sandbox allow-forms";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }


        [Fact]
        public void ScriptSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "script-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void StyleSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "style-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void UpgradeInsecureRequestsIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "upgrade-insecure-requests";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void WorkerSrcIsParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "worker-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }

        [Fact]
        public void MultipleDirectivesAreParsed()
        {
            var options = new CspOptions();
            var originalPolicy = "script-src https://example.org; style-src https://example.org";
            var parser = new CspParser();

            var parsedPolicy = parser.ParsePolicy(originalPolicy);

            Assert.Equal(originalPolicy, parsedPolicy.ToString());
        }
    }
}
