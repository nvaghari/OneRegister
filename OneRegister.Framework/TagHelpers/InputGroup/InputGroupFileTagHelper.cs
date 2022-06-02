using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace OneRegister.Framework.TagHelpers.InputGroup
{
    [HtmlTargetElement("ig-file", TagStructure = TagStructure.WithoutEndTag)]
    public class InputGroupFileTagHelper : TagHelper
    {
        [HtmlAttributeName("for")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("ref")]
        public ModelExpression TargetBack { get; set; }
        [HtmlAttributeName("link")]
        public string Link { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            CheckTargetType();
            output.TagName = "div";
            output.AddClass("form-group", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;
            AddValidation(output);
            AddUpload(output);
            AddRemove(output);
            AddDescription(output);
        }

        private void AddRemove(TagHelperOutput output)
        {
            var inputGroup = new TagBuilder("div");
            inputGroup.MergeAttribute("data-file-for", Target.Metadata.PropertyName);
            if (string.IsNullOrEmpty(TargetBack.Model?.ToString()))
            {
                inputGroup.AddCssClass("input-group d-none");
            }
            else
            {
                inputGroup.AddCssClass("input-group");
            }

            var inputPrepend = new TagBuilder("div");
            inputPrepend.AddCssClass("input-group-prepend");

            var labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            var inputText = new TagBuilder("span");
            inputText.AddCssClass("input-group-text bg-warning");
            inputText.InnerHtml.Append(labelText);
            inputPrepend.InnerHtml.AppendHtml(inputText);
            inputGroup.InnerHtml.AppendHtml(inputPrepend);

            var aTag = new TagBuilder("a");
            aTag.AddCssClass("btn btn-link");
            aTag.MergeAttribute("href", Link);
            aTag.MergeAttribute("target", "_blank");
            aTag.InnerHtml.Append("File has been attached");
            inputGroup.InnerHtml.AppendHtml(aTag);

            var inputAppend = new TagBuilder("div");
            inputAppend.AddCssClass("input-group-append");
            inputAppend.MergeAttribute("onclick", $"OneRegister.event.replaceFileBtn('{Target.Metadata.PropertyName}')");
            var inputAppendText = new TagBuilder("span");
            inputAppendText.AddCssClass("input-group-text btn btn-danger");
            inputAppendText.InnerHtml.Append("Replace");
            inputAppend.InnerHtml.AppendHtml(inputAppendText);

            inputGroup.InnerHtml.AppendHtml(inputAppend);
            output.Content.AppendHtml(inputGroup);

        }

        private void AddValidation(TagHelperOutput output)
        {
            var validation = new TagBuilder("span");
            validation.Attributes.Add("data-val-text", Target.Metadata.PropertyName);
            validation.AddCssClass("font-italic text-danger d-none");
            output.Content.AppendHtml(validation);
        }

        private void CheckTargetType()
        {
            if (Target.Metadata.ModelType.Name != "IFormFile")
            {
                throw new ApplicationException($"{Target.Metadata.PropertyName} is not an IFormFile type");
            }
        }

        private void AddUpload(TagHelperOutput output)
        {

            var inputGroup = new TagBuilder("div");
            inputGroup.MergeAttribute("data-file-for", Target.Metadata.PropertyName);
            if (string.IsNullOrEmpty(TargetBack.Model?.ToString()))
            {
                inputGroup.AddCssClass("input-group");
            }
            else
            {
                inputGroup.AddCssClass("input-group d-none");
            }
            var inputGroupPrepend = new TagBuilder("div");
            inputGroupPrepend.AddCssClass("input-group-prepend");

            var labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            var inputGroupText = new TagBuilder("span");
            inputGroupText.AddCssClass("input-group-text");
            inputGroupText.InnerHtml.Append(labelText);
            inputGroupPrepend.InnerHtml.AppendHtml(inputGroupText);
            inputGroup.InnerHtml.AppendHtml(inputGroupPrepend);

            var customFile = new TagBuilder("div");
            customFile.AddCssClass("custom-file");
            var customFileInput = new TagBuilder("input");
            customFileInput.TagRenderMode = TagRenderMode.StartTag;
            customFileInput.MergeAttribute("type", "file");
            customFileInput.AddCssClass("custom-file-input");
            customFileInput.MergeAttribute("id", Target.Metadata.PropertyName);
            customFileInput.MergeAttribute("name", Target.Metadata.PropertyName);
            customFile.InnerHtml.AppendHtml(customFileInput);

            var label = new TagBuilder("label");
            label.AddCssClass("custom-file-label");
            label.MergeAttribute("for", Target.Metadata.PropertyName);
            label.InnerHtml.Append("Choose File");
            customFile.InnerHtml.AppendHtml(label);

            inputGroup.InnerHtml.AppendHtml(customFile);
            output.Content.AppendHtml(inputGroup);
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
