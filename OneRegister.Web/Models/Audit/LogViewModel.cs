using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OneRegister.Web.Models.Audit
{
    public class LogViewModel
    {
        public string FileListId { get; set; }
        public List<SelectListItem> FileList { get; set; }
    }
}
