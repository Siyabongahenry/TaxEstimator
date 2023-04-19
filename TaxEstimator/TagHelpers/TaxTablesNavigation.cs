using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace TaxEstimator.TagHelpers
{
    
    [HtmlTargetElement("div",Attributes ="tax-tables-navigation")]
    public class TaxTablesNavigation:TagHelper
    {
        public int TaxYear { get; set; }
        public string? IndexUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
           
            int currentTaxYear = DateTime.Now.Year+1;

            TagBuilder result = new TagBuilder("div");

            for (int i= currentTaxYear; i > currentTaxYear - 10;i--)
            {
                TagBuilder a = new TagBuilder("a");

                a.Attributes.Add("href","?year="+i.ToString());

                a.AddCssClass("btn m-2");
                a.AddCssClass(TaxYear == i ? "bg-theme" : "btn-outline-primary");

                a.InnerHtml.Append(i.ToString());

                result.InnerHtml.AppendHtml(a);
       
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
