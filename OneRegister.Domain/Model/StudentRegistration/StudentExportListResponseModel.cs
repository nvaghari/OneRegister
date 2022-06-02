using System.Collections.Generic;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class StudentExportListResponseModel
    {
        public StudentExportListResponseModel()
        {
            Students = new List<StudentExportModel>();
        }
        public bool IsSuccessful { get; set; }
        public string Description { get; set; }
        public List<StudentExportModel> Students { get; set; }
    }
}
