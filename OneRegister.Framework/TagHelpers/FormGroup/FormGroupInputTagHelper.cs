using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OneRegister.Framework.TagHelpers.Enums;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using InputType = OneRegister.Framework.TagHelpers.Enums.InputType;

namespace OneRegister.Framework.TagHelpers.FormGroup
{
    [HtmlTargetElement("form-group-input", TagStructure = TagStructure.WithoutEndTag)]
    public class FormGroupInputTagHelper : TagHelper
    {
        [HtmlAttributeName("target")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("multiline")]
        public int Multiline { get; set; }

        [HtmlAttributeName("auto")]
        public bool AutoComplete { get; set; } = true;

        [HtmlAttributeName("readonly")]
        public bool ReadOnly { get; set; } = false;

        [HtmlAttributeName("type")]
        public InputType Type { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("form-group", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;
            AddLabel(output);
            AddInput(output);
            AddDescription(output);
            AddValidation(output);
        }

        private void AddValidation(TagHelperOutput output)
        {
            var validation = new TagBuilder("div");
            validation.Attributes.Add("data-val-text", Target.Metadata.PropertyName);
            //validation.Attributes.Add("id", Target.Metadata.PropertyName + "Validation");
            validation.AddCssClass("font-italic text-danger d-none");
            output.Content.AppendHtml(validation);
        }

        private void AddLabel(TagHelperOutput output)
        {
            var label = new TagBuilder("label");
            var labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            if (Target.Metadata.IsRequired) labelText += "*";
            label.Attributes.Add("for", Target.Metadata.PropertyName);
            label.InnerHtml.Append(labelText);
            output.Content.AppendHtml(label);
        }

        private void AddInput(TagHelperOutput output)
        {
            TagBuilder input;
            if (Multiline > 0)
            {
                input = new TagBuilder("textarea");
                input.Attributes.Add("rows", Multiline.ToString());
                input.InnerHtml.Append(GetStringValue());
            }
            else
            {
                input = new TagBuilder("input")
                {
                    TagRenderMode = TagRenderMode.StartTag
                };
                input.MergeAttribute("value", GetStringValue());
            }
            if (ReadOnly)
            {
                input.MergeAttribute("readonly", "readonly");
            }
            List<string> validations = FindValidations();
            if (validations.Count > 0)
            {
                input.Attributes.Add("data-validations", string.Join(",", validations));
            }
            input.AddCssClass("form-control");
            input.Attributes.Add("id", Target.Metadata.PropertyName);
            input.Attributes.Add("name", Target.Metadata.PropertyName);
            if (!AutoComplete)
            {
                input.MergeAttribute("autocomplete", "off");
            }
            TypeChecking(input);
            output.Content.AppendHtml(input);
        }

        private void TypeChecking(TagBuilder input)
        {
            switch (Type)
            {
                case InputType.Text:
                    input.MergeAttribute("type", "text");
                    break;
                case InputType.Email:
                    input.MergeAttribute("type", "email");
                    break;
                case InputType.Number:
                    input.MergeAttribute("type", "number");
                    break;
                case InputType.Date:
                    input.MergeAttribute("type", "date");
                    break;
                case InputType.Password:
                    input.MergeAttribute("type", "password");
                    break;
                default:
                    input.MergeAttribute("type", "text");
                    break;
            }
        }

        private string GetStringValue()
        {
            if (Target.Model != null)
            {
                if (Target.Model is decimal)
                {
                    return ((decimal)Target.Model).ToString("F2");
                }
                return Target.Model.ToString();
            }

            return string.Empty;
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
    }
}
