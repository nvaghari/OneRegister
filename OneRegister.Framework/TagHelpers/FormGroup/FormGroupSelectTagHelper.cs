using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OneRegister.Framework.TagHelpers.Enums;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace OneRegister.Framework.TagHelpers.FormGroup
{
    public class FormGroupSelectTagHelper : TagHelper
    {
        private const int THRESHOLD = 10;
        [HtmlAttributeName("for")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("list")]
        public Dictionary<string, string> List { get; set; }
        [HtmlAttributeName("disabled")]
        public bool IsDisable { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("form-group", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Target.Metadata.IsEnum)
            {
                List = GetListFromEnum();
            }
            AddLabel(output);
            AddSelect(output);
            AddDescription(output);
            AddValidation(output);
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
        private void AddSelect(TagHelperOutput output)
        {
            var selectTag = new TagBuilder("select");
            selectTag.AddCssClass("form-control");
            if (List?.Count > THRESHOLD)
            {
                selectTag.AddCssClass("select2");
            }
            selectTag.MergeAttribute("id", Target.Metadata.PropertyName);
            selectTag.MergeAttribute("name", Target.Metadata.PropertyName);
            List<string> validations = FindValidations();
            if (validations.Count > 0)
            {
                selectTag.Attributes.Add("data-validations", string.Join(",", validations));
            }
            if (IsDisable)
            {
                selectTag.Attributes.Add("disabled", "disabled");
            }
            AddSelectOptions(selectTag);
            output.Content.AppendHtml(selectTag);
        }
        private void AddSelectOptions(TagBuilder selectTag)
        {
            string labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            var optionTag = new TagBuilder("option");
            optionTag.MergeAttribute("value", string.Empty);
            optionTag.InnerHtml.Append($"--Select {labelText}--");
            selectTag.InnerHtml.AppendHtml(optionTag);
            foreach (var item in List ?? new Dictionary<string, string>())
            {
                optionTag = new TagBuilder("option");
                optionTag.MergeAttribute("value", item.Key);
                optionTag.InnerHtml.Append(item.Value);
                if (Target.Model != null && item.Key == Target.Model.ToString())
                {
                    optionTag.MergeAttribute("selected", "true");
                }
                selectTag.InnerHtml.AppendHtml(optionTag);
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
            var validation = new TagBuilder("div");
            validation.Attributes.Add("data-val-text", Target.Metadata.PropertyName);
            validation.AddCssClass("font-italic text-danger d-none");
            output.Content.AppendHtml(validation);
        }
        private Dictionary<string, string> GetListFromEnum()
        {
            var list = new Dictionary<string, string>();
            if (Target.Metadata.IsNullableValueType)
            {
                foreach (var name in System.Enum.GetNames(Nullable.GetUnderlyingType(Target.Metadata.ModelType)))
                {
                    list.Add(Convert.ToString((int)Enum.Parse(Nullable.GetUnderlyingType(Target.Metadata.ModelType), name)), name);
                }
                return list;
            }
            else
            {
                foreach (var name in Enum.GetNames(Target.Metadata.ModelType))
                {
                    list.Add(Convert.ToString((int)Enum.Parse(Target.Metadata.ModelType, name)), name);
                }
                return list;
            }

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
