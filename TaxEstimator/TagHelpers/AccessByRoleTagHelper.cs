
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TaxEstimator.TagHelpers
{
    [HtmlTargetElement("Role-Access")]
    public class AccessByRoleTagHelper:TagHelper
    {
        public string? CurrentUserRole { get; set; }
        public string? OwnerRole { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            if(CurrentUserRole != OwnerRole)
            {
                output.Content.SetHtmlContent("");
                output.Content.Clear();
            }

        }

    }
}
