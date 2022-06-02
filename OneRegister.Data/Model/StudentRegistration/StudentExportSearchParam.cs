using System;

namespace OneRegister.Data.Model.StudentRegistration
{
    public class StudentExportSearchParam
    {
        public Guid School { get; set; }
        public int Year { get; set; }
        public Guid Class { get; set; }
        public Guid HomeRoom { get; set; }
    }
}
