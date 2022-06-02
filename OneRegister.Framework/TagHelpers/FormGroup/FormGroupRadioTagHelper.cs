using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Text.Encodings.Web;

namespace OneRegister.Framework.TagHelpers.FormGroup
{
    [HtmlTargetElement("form-group-radio", TagStructure = TagStructure.WithoutEndTag)]
    public class FormGroupRadioTagHelper : TagHelper
    {
        [HtmlAttributeName("target")]
        public ModelExpression Target { get; set; }
        [HtmlAttributeName("inline")]
        public bool Inline { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("form-group", HtmlEncoder.Default);
            output.TagMode = TagMode.StartTagAndEndTag;

            AddRadioItems(output);
        }

        private void AddRadioItems(TagHelperOutput output)
        {
            var labelText = string.IsNullOrEmpty(Target.Metadata.DisplayName) ? Target.Name : Target.Metadata.DisplayName;
            if (Target.Metadata.IsRequired) labelText += "*";
            var fieldSet = new TagBuilder("fieldset");
            fieldSet.Attributes.Add("id", Target.Metadata.PropertyName);

            var legend = new TagBuilder("legend");
            legend.AddCssClass("col-form-label");
            legend.InnerHtml.Append(labelText);
            fieldSet.InnerHtml.AppendHtml(legend);

            foreach (var item in Target.Metadata.EnumNamesAndValues.OrderByDescending(i => i.Value))
            {
                AddOption(fieldSet, item);
            }

            output.Content.AppendHtml(fieldSet);

        }

        private void AddOption(TagBuilder fieldSet, System.Collections.Generic.KeyValuePair<string, string> item)
        {
            var holder = new TagBuilder("div");
            holder.AddCssClass("form-check form-check-inline");

            var radio = new TagBuilder("input");
            radio.TagRenderMode = TagRenderMode.StartTag;
            radio.Attributes.Add("type", "radio");
            radio.AddCssClass("form-check-input");

            var label = new TagBuilder("label");
            label.AddCssClass("form-check-label");

            radio.Attributes.Add("name", Target.Metadata.PropertyName);
            radio.Attributes.Add("id", item.Key);
            radio.Attributes.Add("value", item.Value);
            if (Target.Model?.ToString() == item.Key)
            {
                radio.MergeAttribute("checked", "true");
            }
            label.InnerHtml.Append(item.Key);
            label.Attributes.Add("for", item.Key);

            holder.InnerHtml.AppendHtml(radio);
            holder.InnerHtml.AppendHtml(label);
            fieldSet.InnerHtml.AppendHtml(holder);
        }
    }
}
