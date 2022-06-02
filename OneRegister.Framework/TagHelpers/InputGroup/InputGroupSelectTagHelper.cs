using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OneRegister.Framework.TagHelpers.Enums;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace OneRegister.Framework.TagHelpers.InputGroup
{
    [HtmlTargetElement("ig-select", TagStructure = TagStructure.WithoutEndTag)]
    public class InputGroupSelectTagHelper : TagHelper
    {
        private const int THRESHOLD = 10;
        [HtmlAttributeName("for")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("icon")]
        public string Icon { get; set; }
        [HtmlAttributeName("list")]
        public Dictionary<string, string> List { get; set; }
        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("form-group", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Target.Metadata.IsEnum)
            {
                List = GetListFromEnum();
            }
            AddValidation(output);
            AddSelect(output);
            AddDescription(output);
        }

        private void AddSelect(TagHelperOutput output)
        {
            var inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("input-group flex-nowrap");

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
            string inputText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            inputGroupPrepend.MergeAttribute("data-toggle", "tooltip");
            inputGroupPrepend.MergeAttribute("title", inputText);

            var fontIcon = new TagBuilder("i");
            fontIcon.AddCssClass($"fas {Icon}");
            inputGroupPrepend.InnerHtml.AppendHtml(fontIcon);
            inputGroup.InnerHtml.AppendHtml(inputGroupPrepend);

            var selectTag = new TagBuilder("select");
            selectTag.AddCssClass("form-control");
            if (List?.Count > THRESHOLD)
            {
                selectTag.AddCssClass("select2");
            }
            if (Multiple)
            {
                selectTag.AddCssClass("select2");
                selectTag.MergeAttribute("multiple", "multiple");
            }
            selectTag.MergeAttribute("id", Target.Name);
            selectTag.MergeAttribute("name", Target.Name);

            //validation
            List<string> validations = FindValidations();
            if (validations.Count > 0)
            {
                selectTag.Attributes.Add("data-validations", string.Join(",", validations));
            }

            AddSelectOptions(selectTag);

            inputGroup.InnerHtml.AppendHtml(selectTag);
            output.Content.AppendHtml(inputGroup);
        }

        private void AddSelectOptions(TagBuilder selectTag)
        {
            string labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            TagBuilder optionTag;
            optionTag = new TagBuilder("option");
            optionTag.MergeAttribute("value", string.Empty);
            optionTag.InnerHtml.Append($"--Select {labelText}--");
            //if (Target.Metadata.IsEnum)
            //{
            //    List = GetListFromEnum();
            //}
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

        private Dictionary<string, string> GetListFromEnum()
        {
            var list = new Dictionary<string, string>();
            if (Target.Metadata.IsNullableValueType)
            {
                foreach (var name in Enum.GetNames(Nullable.GetUnderlyingType(Target.Metadata.ModelType)))
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

        private void AddValidation(TagHelperOutput output)
        {
            var validation = new TagBuilder("span");
            validation.Attributes.Add("data-val-text", Target.Metadata.PropertyName);
            validation.AddCssClass("font-italic text-danger d-none");
            output.Content.AppendHtml(validation);
        }
        private void AddDescription(TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Target.Metadata.Description)) return;
            var small = new TagBuilder("small");
            small.AddCssClass("form-text text-muted");
            small.InnerHtml.Append(Target.Metadata.Description);
            output.Content.AppendHtml(small);
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
