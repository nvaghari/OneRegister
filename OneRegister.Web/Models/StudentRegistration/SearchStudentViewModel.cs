using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Web.Models.StudentRegistration
{
    public class SearchStudentViewModel
    {
        public SearchStudentViewModel()
        {
            IsNew = false;
        }
        public string StudentNumber { get; set; }
        [Display(Name = "School")]
        public Guid SchoolId { get; set; }
        public List<SelectListItem> Schools { get; set; }
        public bool IsNew { get; set; }
    }
}
