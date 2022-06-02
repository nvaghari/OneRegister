using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OneRegister.Framework.TagHelpers.Enums;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using InputType = OneRegister.Framework.TagHelpers.Enums.InputType;

namespace OneRegister.Framework.TagHelpers.InputGroup
{
    [HtmlTargetElement("ig-text", TagStructure = TagStructure.WithoutEndTag)]
    public class InputGroupTextTagHelper : TagHelper
    {
        [HtmlAttributeName("for")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("multi")]
        public int Multiline { get; set; }
        [HtmlAttributeName("icon")]
        public string Icon { get; set; }
        [HtmlAttributeName("auto")]
        public bool AutoComplete { get; set; } = true;
        [HtmlAttributeName("type")]
        public InputType Type { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Type == InputType.Hidden)
            {
                AddHiddenInput(output);
            }
            else
            {
                output.TagName = "div";
                output.AddClass("form-group", HtmlEncoder.Default);
                output.TagMode = TagMode.StartTagAndEndTag;
                AddValidation(output);
                AddInput(output);
                AddDescription(output);
            }
        }
        private void AddHiddenInput(TagHelperOutput output)
        {
            output.TagName = "input";
            output.Attributes.Add("value", GetStringValue());
            output.Attributes.Add("name", Target.Metadata.PropertyName);
            output.Attributes.Add("id", Target.Metadata.PropertyName);
            output.Attributes.Add("type", "hidden");
        }
        private void AddInput(TagHelperOutput output)
        {
            var inputText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            var inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("input-group");

            var inputGroupPrepend = new TagBuilder("div");
            if (Target.Model == null || string.IsNullOrEmpty(Target.Model.ToString()))
            {
                inputGroupPrepend.AddCssClass("input-group-prepend input-group-text");
            }
            else
            {
                inputGroupPrepend.AddCssClass("input-group-prepend input-group-text bg-warning");
            }

            //tooltip
            inputGroupPrepend.MergeAttribute("data-toggle", "tooltip");
            inputGroupPrepend.MergeAttribute("title", inputText);

            var fontIcon = new TagBuilder("i");
            fontIcon.AddCssClass($"fas {Icon}");
            inputGroupPrepend.InnerHtml.AppendHtml(fontIcon);
            inputGroup.InnerHtml.AppendHtml(inputGroupPrepend);


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
                input.MergeAttribute("type", GetInputType(Type));
            }

            //validation
            List<string> validations = FindValidations();
            if (validations.Count > 0)
            {
                input.Attributes.Add("data-validations", string.Join(",", validations));
            }

            input.MergeAttribute("placeholder", inputText);
            input.AddCssClass("form-control");
            input.MergeAttribute("name", Target.Name);
            input.MergeAttribute("id", Target.Name);
            if (!AutoComplete)
            {
                input.MergeAttribute("autocomplete", "off");
            }
            TypeChecking(input);
            inputGroup.InnerHtml.AppendHtml(input);


            output.Content.AppendHtml(inputGroup);
        }

        private static string GetInputType(InputType type)
        {
            return type switch
            {
                InputType.Text => "text",
                InputType.Hidden => "hidden",
                InputType.Email => "email",
                InputType.Number => "number",
                InputType.Date => "date",
                InputType.Password => "password",
                InputType.Tel => "tel",
                InputType.URL => "url",
                _ => "text",
            };
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
                default:
                    input.MergeAttribute("type", "text");
                    break;
            }
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
            var validation = new TagBuilder("span");
            validation.Attributes.Add("data-val-text", Target.Metadata.PropertyName);
            validation.AddCssClass("font-italic text-danger d-none");
            output.Content.AppendHtml(validation);
        }
        private string GetStringValue()
        {
            if (Type == InputType.Date)
            {
                return Target.Model == null ? string.Empty : Convert.ToDateTime(Target.Model).ToString("yyyy-MM-dd");
            }
            return Target.Model == null ? string.Empty : Convert.ToString(Target.Model);
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
    }
}
