using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApplication1.TagHelpers
{
    [HtmlTargetElement("book-availability")]
    public class BookAvailabilityTagHelper : TagHelper
    {
        public string Status { get; set; } = string.Empty;
        public bool IsRestricted { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            bool isAvailable = Status.Equals("Available", StringComparison.OrdinalIgnoreCase);

            if (isAvailable)
            {
                output.Attributes.SetAttribute("class", "badge bg-success");
                output.Content.SetContent("Available");
            }
            else
            {
                output.Attributes.SetAttribute("class", "badge bg-danger");
                output.Content.SetContent("Not Available");
            }
        }
    }
}