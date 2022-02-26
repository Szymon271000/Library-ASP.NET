using Library.Models.ValueObjects;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Customizations.TagHelpers
{
    public class PriceTagHelper : TagHelper
    {
        public Money CurrentPrice { get; set; }
        public Money FullPrice { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.Content.AppendHtml($"{CurrentPrice}");
            if(!CurrentPrice.Equals(FullPrice))
            {
                output.Content.AppendHtml($"<br><s>{FullPrice}</s>");
            }
        }
    }
}
