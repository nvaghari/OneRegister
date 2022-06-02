using System;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class StudentListModel
    {
        public Guid Id { get; set; }
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public bool HasPicture { get; set; }
        public string IdentityType { get; set; }
        public string StudentNumber { get; set; }
        public string School { get; set; }
        public string Status { get; set; }
    }
}
