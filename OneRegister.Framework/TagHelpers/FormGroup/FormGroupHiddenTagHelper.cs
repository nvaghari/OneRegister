using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace OneRegister.Framework.TagHelpers.FormGroup
{
    [HtmlTargetElement("form-group-hidden", TagStructure = TagStructure.WithoutEndTag)]
    public class FormGroupHiddenTagHelper : TagHelper
    {
        [HtmlAttributeName("target")]
        public ModelExpression Target { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.Attributes.Add("value", GetStringValue());
            output.Attributes.Add("name", Target.Metadata.PropertyName);
            output.Attributes.Add("id", Target.Metadata.PropertyName);
            output.Attributes.Add("type", "hidden");

        }
        private string GetStringValue()
        {
            return Target.Model == null ? string.Empty : Convert.ToString(Target.Model);
        }

    }
}
