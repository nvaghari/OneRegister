using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.ExportPhotoApp.Model
{
    public class SchoolYearModel
    {
        public Dictionary<string,string> Schools { get; set; }
        public List<TextValueItem> SchoolsList => Schools.Select(d => new TextValueItem { Text = d.Value, Value = d.Key }).ToList();
        public Dictionary<string, string> Years { get; set; }
        public List<TextValueItem> YearsList => Years.Select(d => new TextValueItem {Text= d.Value,Value = d.Key }).ToList();
    }
}
