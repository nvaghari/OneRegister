using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OneRegister.Framework.TagHelpers.Enums;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace OneRegister.Framework.TagHelpers.FormGroup
{
    [HtmlTargetElement("form-group-file", TagStructure = TagStructure.WithoutEndTag)]
    public class FormGroupFileTagHelper : TagHelper
    {
        [HtmlAttributeName("target")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("handler")]
        public string Handler { get; set; }
        [HtmlAttributeName("link")]
        public string Link { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("form-group", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;
            AddLabel(output);
            AddUploadInput(output);
            AddRemoveInput(output);
            AddDescription(output);
            AddValidation(output);
        }
        private void AddLabel(TagHelperOutput output)
        {
            var label = new TagBuilder("label");
            var labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            if (Target.Metadata.IsRequired) labelText += "*";
            label.MergeAttribute("for", Target.Metadata.PropertyName);
            //label.Attributes.Add("for", Target.Metadata.PropertyName);
            label.InnerHtml.Append(labelText);
            output.Content.AppendHtml(label);
        }
        private void AddUploadInput(TagHelperOutput output)
        {
            var inputGroup = new TagBuilder("div");
            var hasFile = !string.IsNullOrEmpty(Target.Model?.ToString());
            if (hasFile)
            {
                inputGroup.AddCssClass("input-group d-none");

            }
            else
            {
                inputGroup.AddCssClass("input-group");
            }
            inputGroup.MergeAttribute("id", $"{Target.Metadata.PropertyName}UpHolder");
            var inputGroupAppend = new TagBuilder("div");
            inputGroupAppend.AddCssClass("input-group-append");
            //inputGroupAppend.MergeAttribute("id", Target.Metadata.PropertyName + "Btn");
            inputGroupAppend.MergeAttribute("onclick", Handler + $"('{Target.Metadata.PropertyName}',1)");
            var inputGroupAppendSpan = new TagBuilder("span");
            inputGroupAppendSpan.AddCssClass("input-group-text btn btn-info");
            inputGroupAppendSpan.InnerHtml.Append("Upload");
            inputGroupAppend.InnerHtml.AppendHtml(inputGroupAppendSpan);

            var customFile = new TagBuilder("div");
            customFile.AddCssClass("custom-file");
            var input = new TagBuilder("input")
            {
                TagRenderMode = TagRenderMode.StartTag
            };
            List<string> validations = FindValidations();
            if (validations.Count > 0)
            {
                input.MergeAttribute("data-validations", string.Join(",", validations));
            }
            input.AddCssClass("custom-file-input");
            input.MergeAttribute("id", Target.Metadata.PropertyName);
            input.MergeAttribute("type", "file");
            if (hasFile)
            {
                input.MergeAttribute("data-file-id", Target.Model?.ToString());
            }
            customFile.InnerHtml.AppendHtml(input);
            var customFileLabel = new TagBuilder("span");
            customFileLabel.AddCssClass("custom-file-label");
            customFileLabel.MergeAttribute("for", Target.Metadata.PropertyName);
            customFile.InnerHtml.AppendHtml(customFileLabel);

            inputGroup.InnerHtml.AppendHtml(customFile);
            inputGroup.InnerHtml.AppendHtml(inputGroupAppend);
            output.Content.AppendHtml(inputGroup);
        }
        private void AddRemoveInput(TagHelperOutput output)
        {
            var inputGroup = new TagBuilder("div");
            if (string.IsNullOrEmpty(Target.Model?.ToString()))
            {
                inputGroup.AddCssClass("input-group d-none");
            }
            else
            {
                inputGroup.AddCssClass("input-group");
            }
            inputGroup.MergeAttribute("id", $"{Target.Metadata.PropertyName}Holder");

            var fileLink = new TagBuilder("a");
            fileLink.MergeAttribute("href", Link);
            fileLink.MergeAttribute("target", "_blank");
            fileLink.AddCssClass("btn btn-link");
            fileLink.InnerHtml.Append("File is Uploaded");

            var inputGroupAppend = new TagBuilder("div");
            inputGroupAppend.AddCssClass("input-group-append");
            //inputGroupAppend.MergeAttribute("id", Target.Metadata.PropertyName + "RemoveBtn");
            inputGroupAppend.MergeAttribute("onclick", Handler + $"('{Target.Metadata.PropertyName}',0)");
            var inputGroupAppendSpan = new TagBuilder("span");
            inputGroupAppendSpan.AddCssClass("input-group-text btn btn-danger");
            inputGroupAppendSpan.InnerHtml.Append("Remove");
            inputGroupAppend.InnerHtml.AppendHtml(inputGroupAppendSpan);



            inputGroup.InnerHtml.AppendHtml(fileLink);
            inputGroup.InnerHtml.AppendHtml(inputGroupAppend);
            output.Content.AppendHtml(inputGroup);
        }
        private List<string> FindValidations()
        {
            var validations = new List<string>();
            if (Target.Metadata.IsRequired)
            {
                validations.Add(ValidationType.Required.ToString());
            }
            if (!string.IsNullOrEmpty(Target.Metadata.DataTypeName))
            {
                validations.Add(Target.Metadata.DataTypeName);
            }
            return validations;
        }
        private void AddDescription(TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Target.Metadata.Description)) return;
            var small = new TagBuilder("small");
            small.AddCssClass("form-text text-muted");
            small.InnerHtml.Append(Target.Metadata.Description);
            output.Content.AppendHtml(small);
        }
        private void AddValidation(TagHelperOutput output)
        {
            var validation = new TagBuilder("div");
            validation.Attributes.Add("data-val-text", Target.Metadata.PropertyName);
            validation.AddCssClass("font-italic text-danger d-none");
            output.Content.AppendHtml(validation);
        }
    }
}
