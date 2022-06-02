using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace OneRegister.Framework.TagHelpers.FormGroup
{
    [HtmlTargetElement("form-check", TagStructure = TagStructure.WithoutEndTag)]
    public class FormCheckTagHelper : TagHelper
    {
        [HtmlAttributeName("target")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("SimpleBinding")]
        public bool SimpleBinding { get; set; } = false;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("custom-control", HtmlEncoder.Default);
            output.AddClass("custom-checkbox", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;
            AddInput(output);
            AddLabel(output);
            AddDescription(output);
            AddValidation(output);
        }
        private string _targetName => SimpleBinding ? Target.Metadata.PropertyName : Target.Name;
        private void AddValidation(TagHelperOutput output)
        {
            var validation = new TagBuilder("div");
            validation.Attributes.Add("data-val-text", _targetName);
            validation.AddCssClass("font-italic text-danger d-none");
            output.Content.AppendHtml(validation);
        }

        private void AddLabel(TagHelperOutput output)
        {
            var label = new TagBuilder("label");
            var labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            label.Attributes.Add("for", _targetName);
            label.AddCssClass("custom-control-label");
            label.InnerHtml.Append(labelText);
            output.Content.AppendHtml(label);
        }

        private void AddInput(TagHelperOutput output)
        {
            TagBuilder input;

            input = new TagBuilder("input")
            {
                TagRenderMode = TagRenderMode.StartTag
            };
            if ((Target.Model as bool?).HasValue && (Target.Model as bool?).Value)
            {
                input.Attributes.Add("checked", string.Empty);
            }
            input.MergeAttribute("value", "true");
            input.AddCssClass("custom-control-input");
            input.Attributes.Add("id", _targetName);
                input.Attributes.Add("name", _targetName);
            
            input.Attributes.Add("type", "checkbox");
            output.Content.AppendHtml(input);
        }

        private void AddDescription(TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Target.Metadata.Description)) return;
            var small = new TagBuilder("small");
            small.AddCssClass("form-text text-muted");
            small.InnerHtml.Append(Target.Metadata.Description);
            output.Content.AppendHtml(small);
        }
    }
}
