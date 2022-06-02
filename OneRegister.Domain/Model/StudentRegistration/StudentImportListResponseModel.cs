using System.Collections.Generic;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class StudentImportListResponseModel

    {
        public StudentImportListResponseModel()
        {
            Students = new List<StudentImportModel>();
        }
        public static StudentImportListResponseModel Failure(string description)
        {
            return new StudentImportListResponseModel
            {
                IsSuccessful = false,
                Description = description
            };
        }
        public static StudentImportListResponseModel Success(List<StudentImportModel> students)
        {
            return new StudentImportListResponseModel
            {
                IsSuccessful = true,
                Students = students
            };
        }
        public static StudentImportListResponseModel Success(string description)
        {
            return new StudentImportListResponseModel
            {
                IsSuccessful = true,
                Description = description
            };
        }
        public bool IsSuccessful { get; set; }
        public string Description { get; set; }
        public List<StudentImportModel> Students { get; set; }
    }
}
