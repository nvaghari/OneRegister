using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.ExportPhoto
{
    public class ExportSchoolYears
    {
        public Dictionary<string,string> Schools { get; set; }
        public Dictionary<string,string> Years { get; set; }
    }
}
