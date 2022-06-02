using CsvHelper.Configuration;

namespace OneRegister.Domain.Model.AgropreneurRegistration
{
    public class AGPCsvFileMapper : ClassMap<AGPImportModel>
    {
        public AGPCsvFileMapper()
        {
            Map(X => X.PlotNo).Name("ID/PLOT NO").Index(0);
            Map(X => X.FirstName).Name("FIRST NAME").Index(1);
            Map(X => X.LastName).Name("LAST NAME").Index(2);
            Map(X => X.IdentityNumber).Name("NRIC").Index(3);
            Map(X => X.Company).Name("COMPANY").Index(4);
            Map(X => X.SsmNo).Name("SSM NO").Index(5);
            Map(X => X.MailingAddress).Name("MAILING ADDRESS").Index(6);
            Map(X => X.DateOfBirth).Name("DATE OF BIRTH").Index(7);
            Map(X => X.Gender).Name("GENDER").Index(8);
            Map(X => X.Nationality).Name("NATIONALITY").Index(9);
            Map(X => X.MobileNo).Name("MOBILE NO").Index(10);
            Map(X => X.EmailAddress).Name("EMAIL ADDRESS").Index(11);
            Map(X => X.Designation).Name("DESIGNATION").Index(12);
            Map(X => X.Industry).Name("INDUSTRY").Index(13);
            Map(X => X.NatureOfBusiness).Name("NATURE OF BUSINESS").Index(14);
            Map(X => X.PurposeOfTransaction).Name("PURPOSE OF TRANSACTION").Index(15);
            Map(X => X.CompanyBankAccount).Name("COMPANY BANK ACCOUNT").Index(16);
            Map(X => X.AccountNo).Name("ACCOUNT NO").Index(17);
        }
    }
}
