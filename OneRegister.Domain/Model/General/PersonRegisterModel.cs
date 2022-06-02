using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.General
{
    public class PersonRegisterModel
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public string Name => FirstName + " " + LastName;

        [Display(Name = "First Name")]
        [CustomRequired]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [CustomRequired]
        public string LastName { get; set; }

        [Display(Name = "Identity Number", Description = "Please enter IC number without \"-\" mark")]
        [CustomRequired]
        public string Identitynumber { get; set; }

        [Display(Name = "Mailing Address")]
        [CustomRequired]
        public string MailingAddress { get; set; }

        [Display(Name = "Date Of Birth")]
        [CustomRequired]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Gender")]
        [CustomRequired]
        public Gender? Gender { get; set; }

        [Display(Name = "Nationality")]
        [CustomRequired]
        public string Nationality { get; set; }
        public Dictionary<string, string> NationalityList { get; set; }

        [Display(Name = "Identity Type")]
        [CustomRequired]
        public string IdentityType { get; set; }
        public Dictionary<string, string> IdentityTypeList { get; set; }

        [Display(Name = "Mobile Number")]
        [CustomRequired]
        [Number]
        public string Mobile { get; set; }

        [Display(Name = "Email Address")]
        [CustomRequired]
        [EmailAddress(ErrorMessage = "Email Address is Not Valid")]
        public string Email { get; set; }
    }
}
