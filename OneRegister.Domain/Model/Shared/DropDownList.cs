using System.Collections.Generic;

namespace OneRegister.Domain.Model.Shared
{
    public class DropDownList
    {
        public DropDownList()
        {
            ListItems = new List<DropDownListItem>();
        }
        public List<DropDownListItem> ListItems { get; set; }

    }
}
