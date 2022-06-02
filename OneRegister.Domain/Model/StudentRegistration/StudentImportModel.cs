using OneRegister.Domain.Validation;
using OneRegister.Domain.Validation.Attributes;
using OneRegister.Domain.Validation.StudentRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class StudentImportModel
    {
        [CustomRequired]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string Service { get; set; }

        [CustomRequired]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        [SchoolCheck]
        public string School { get; set; }

        [CustomRequired]
        [Range(1999, 2999)]
        public string Year { get; set; }

        [CustomRequired]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string Class { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        [ClassLabelCheck]
        public string ClassLabel { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string HomeRoom { get; set; }

        [MaxLength(20, ErrorMessage = "Maximum Length is 20")]
        [StudentNumberUniqueness]
        public string StudentNumber { get; set; }

        [CustomRequired]
        [MaxLength(100, ErrorMessage = "Maximum Length is 100")]
        [StudentNameUniqueness]
        public string Name { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.GENDER)]
        public string Gender { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.NATIONALITY)]
        public string Nationality { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.IDENTITYTYPE)]
        public string IdentityType { get; set; }

        [CustomRequired]
        [ICNumber]
        [StudentIdentityNumberUniqueness]
        public string IdentityNumber { get; set; }


        public DateTime? Birthday { get; set; }
        public string BirthdayString
        {
            get
            {
                return Birthday?.ToString("yyyy-MM-dd");
            }
        }

        [MaxLength(100, ErrorMessage = "Maximum Length is 100")]
        public string ParentName { get; set; }

        [Number]
        public string ParentPhone { get; set; }

        [MaxLength(250, ErrorMessage = "Maximum Length is 250")]
        public string Address { get; set; }


        public bool IsAcceptable { get; set; }
        public string Description { get; set; }
    }
}
