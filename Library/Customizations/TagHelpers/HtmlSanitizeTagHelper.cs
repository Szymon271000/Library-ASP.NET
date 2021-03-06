using AngleSharp.Html.Dom;
using Ganss.XSS;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Customizations.TagHelpers
{
    [HtmlTargetElement(Attributes ="html-sanitize")]
    public class HtmlSanitizeTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            TagHelperContent tagHelperContent = await output.GetChildContentAsync(NullHtmlEncoder.Default);
            string content = tagHelperContent.GetContent(NullHtmlEncoder.Default);
            var sanitizer = CreateSanitizer();
            content = sanitizer.Sanitize(content);
            output.Content.SetHtmlContent(content);
        }
        private static HtmlSanitizer CreateSanitizer()
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.Add("b");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("br");
            sanitizer.AllowedTags.Add("ul");
            sanitizer.AllowedTags.Add("ol");
            sanitizer.AllowedTags.Add("li");
            sanitizer.AllowedTags.Add("iframe");
            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedAttributes.Add("src");
            sanitizer.AllowDataAttributes = false;
            sanitizer.AllowedCssProperties.Clear();
            sanitizer.FilterUrl += FilterUrl;
            sanitizer.PostProcessNode += ProcessIFrames;
            return sanitizer;
        }
        private static void FilterUrl(object sender, FilterUrlEventArgs filterUrlEventArgs)
        {
            if (!filterUrlEventArgs.OriginalUrl.StartsWith("//www.youtube.com/") && !filterUrlEventArgs.OriginalUrl.StartsWith("https://www.youtube.com/"))
            {
                filterUrlEventArgs.SanitizedUrl = null;
            }
        }
        private static void ProcessIFrames(object sender, PostProcessNodeEventArgs postProcessNodeEventArgs)
        {
            var iframe = postProcessNodeEventArgs.Node as IHtmlInlineFrameElement;
            if (iframe == null)
            {
                return;
            }
            var container = postProcessNodeEventArgs.Document.CreateElement("span");
            container.ClassName = "video-container";
            container.AppendChild(iframe.Clone(true));
            postProcessNodeEventArgs.ReplacementNodes.Add(container);
        }
    }
}
