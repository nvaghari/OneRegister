using System;

namespace OneRegister.Data.Entities.EDuit
{
    public class StudentConfirmDataModel
    {
        public Guid Id { get; set; }
        public string StudentNumber { get; set; }
        public string SchoolName { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string IdentityType { get; set; }
        public char Gender { get; set; }
        public string Nationality { get; set; }
        public string IdentityNumber { get; set; }
        public string Birthday { get; set; }
        public string ClassName { get; set; }
        public string ClassLabel { get; set; }
        public string HomeRoomName { get; set; }
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
        public string Address { get; set; }
        public long? DmsRef { get; set; }
    }
}
