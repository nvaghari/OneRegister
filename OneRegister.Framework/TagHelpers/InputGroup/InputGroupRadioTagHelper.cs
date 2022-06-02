using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace OneRegister.Framework.TagHelpers.InputGroup
{
    [HtmlTargetElement("ig-radio", TagStructure = TagStructure.WithoutEndTag)]
    public class InputGroupRadioTagHelper : TagHelper
    {
        [HtmlAttributeName("for")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("icon")]
        public string Icon { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            CheckTargetType();
            output.TagName = "div";
            output.AddClass("form-group", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;
            AddValidation(output);
            AddRadioItems(output);
            AddDescription(output);
        }

        private void AddRadioItems(TagHelperOutput output)
        {
            var labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            if (Target.Metadata.IsRequired) labelText += "*";
            var fieldSet = new TagBuilder("fieldset");
            fieldSet.Attributes.Add("id", Target.Metadata.PropertyName);

            var legend = new TagBuilder("legend");

            if (Target.Model == null || string.IsNullOrEmpty(Target.Model.ToString()))
            {
                legend.AddCssClass("input-group-text");
            }
            else
            {
                legend.AddCssClass("input-group-text bg-warning");
            }

            var fontIcon = new TagBuilder("i");
            fontIcon.AddCssClass($"fas {Icon} mr-2");
            legend.InnerHtml.AppendHtml(fontIcon);
            legend.InnerHtml.Append(labelText);
            fieldSet.InnerHtml.AppendHtml(legend);

            foreach (var item in Target.Metadata.EnumNamesAndValues.OrderByDescending(i => i.Value))
            {
                AddOption(fieldSet, item);
            }

            output.Content.AppendHtml(fieldSet);
        }
        private void AddOption(TagBuilder fieldSet, KeyValuePair<string, string> item)
        {
            var holder = new TagBuilder("div");
            holder.AddCssClass("form-check form-check-inline");

            var radio = new TagBuilder("input");
            radio.TagRenderMode = TagRenderMode.StartTag;
            radio.Attributes.Add("type", "radio");
            radio.AddCssClass("form-check-input");

            var label = new TagBuilder("label");
            label.AddCssClass("form-check-label");

            radio.MergeAttribute("name", Target.Metadata.PropertyName);
            radio.MergeAttribute("id", item.Key);
            radio.MergeAttribute("value", item.Value);
            if (Target.Model?.ToString() == item.Key)
            {
                radio.MergeAttribute("checked", "true");
            }
            label.InnerHtml.Append(item.Key);
            label.MergeAttribute("for", item.Key);

            holder.InnerHtml.AppendHtml(radio);
            holder.InnerHtml.AppendHtml(label);
            fieldSet.InnerHtml.AppendHtml(holder);
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
        private void CheckTargetType()
        {
            if (!Target.Metadata.IsEnum)
            {
                throw new ApplicationException($"{Target.Metadata.PropertyName} should be Enumeration type");
            }
        }
    }
}
