using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Web.Models.Account
{
    public class RoleAddViewModel
    {
        [CustomRequired]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        [CustomRequired]
        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }
        public Dictionary<string, string> Organizations { get; set; }
    }
}
