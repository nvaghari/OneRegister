using CsvHelper.Configuration;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class StudentCsvFileMapper : ClassMap<StudentImportModel>
    {
        public StudentCsvFileMapper()
        {
            Map(x => x.Service).Name("Service").Index(0);
            Map(x => x.School).Name("School").Index(1);
            Map(x => x.Year).Name("Year").Index(2);
            Map(x => x.Class).Name("Class").Index(3);
            Map(x => x.ClassLabel).Name("ClassLabel").Index(4);
            Map(x => x.HomeRoom).Name("HomeRoom").Index(5);
            Map(x => x.StudentNumber).Name("StudentNumber").Index(6);
            Map(x => x.Name).Name("Name").Index(7);
            Map(x => x.Gender).Name("Gender").Index(8);
            Map(x => x.Nationality).Name("Nationality").Index(9);
            Map(x => x.IdentityType).Name("IdentityType").Index(10);
            Map(x => x.IdentityNumber).Name("IdentityNumber").Index(11);
            Map(x => x.Birthday).Name("Birthday").Index(12);
            Map(x => x.ParentName).Name("ParentName").Index(13);
            Map(x => x.ParentPhone).Name("ParentPhone").Index(14);
            Map(x => x.Address).Name("Address").Index(15);
        }
    }
}
