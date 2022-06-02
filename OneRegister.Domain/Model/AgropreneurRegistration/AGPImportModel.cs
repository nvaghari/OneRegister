using OneRegister.Domain.Model.Shared;
using OneRegister.Domain.Validation;
using OneRegister.Domain.Validation.AgropreneurRegistration;
using OneRegister.Domain.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.AgropreneurRegistration
{
    public class AGPImportModel : ImportExcelRowValidationModel
    {
        [CustomRequired]
        [MaxLength(20, ErrorMessage = "Maximum Length is 20")]
        [Number]
        [PlotNoUniqueness]
        public string PlotNo { get; set; }

        [CustomRequired]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string FirstName { get; set; }

        [CustomRequired]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string LastName { get; set; }

        [CustomRequired]
        [MaxLength(20, ErrorMessage = "Maximum Length is 20")]
        [ICNumber]
        public string IdentityNumber { get; set; }

        [CustomRequired]
        [MaxLength(20, ErrorMessage = "Maximum Length is 20")]
        [Collection(AcceptableCodeLists.IDENTITYTYPE)]
        public string IdentityType { get; set; }

        [CustomRequired]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string Company { get; set; }

        [CustomRequired]
        [MaxLength(20, ErrorMessage = "Maximum Length is 20")]
        public string SsmNo { get; set; }

        [CustomRequired]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string MailingAddress { get; set; }

        [CustomRequired]
        [DateString]
        public string DateOfBirth { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.GENDER)]
        public string Gender { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.NATIONALITY)]
        public string Nationality { get; set; }

        [CustomRequired]
        [Number]
        public string MobileNo { get; set; }

        [CustomRequired]
        [Email]
        public string EmailAddress { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.DESIGNATION)]
        public string Designation { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.INDUSTRY)]
        public string Industry { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.DESIGNATION)]
        public string NatureOfBusiness { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.PURPOSETXN)]
        public string PurposeOfTransaction { get; set; }

        [CustomRequired]
        [Collection(AcceptableCodeLists.BANK)]
        public string CompanyBankAccount { get; set; }

        [CustomRequired]
        [Number]
        public string AccountNo { get; set; }

        [DateString]
        public string EntryDate { get; set; }

        [DateString]
        public string VisaExpiry { get; set; }

        [DateString]
        public string PlksExpiry { get; set; }

        [Number]
        public int? TermOfService { get; set; }
    }
}
