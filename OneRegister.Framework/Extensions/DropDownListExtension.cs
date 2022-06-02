using Microsoft.AspNetCore.Mvc.Rendering;
using OneRegister.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Web.Services.Extensions
{
    public static class DropDownListExtension
    {

        public static List<TextValueModel> ToTextValue(this List<SelectListItem> selectListItems)
        {
            return selectListItems.Select(i => new TextValueModel
            {
                Text = i.Text,
                Value = i.Value
            }).ToList();
        }
    }
}
