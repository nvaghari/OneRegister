using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OneRegister.Web.Models.StudentRegistration
{
    public class ExportViewModel
    {
        public List<SelectListItem> Schools { get; set; }
        public List<SelectListItem> Years { get; set; }
    }
}
