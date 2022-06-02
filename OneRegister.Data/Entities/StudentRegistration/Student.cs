using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.ComponentModel.DataAnnotations;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.StudentRegistration
{
    public class Student : Member
    {
        private Guid? _schoolId;
        [StringLength(DataLength.IDENTITY)]
        public string StudentNumber { get; set; }
        [StringLength(DataLength.LONGNAME)]
        public string ParentName { get; set; }
        [StringLength(DataLength.PHONE)]
        public string ParentPhone { get; set; }


        public School School { get; set; }
        public Guid? SchoolId
        {
            get
            {
                return _schoolId;
            }
            set
            {
                OrganizationId = value.Value;
                _schoolId = value;
            }
        }
        public ClassRoom ClassRoom { get; set; }
        public Guid? ClassRoomId { get; set; }
        public HomeRoom HomeRoom { get; set; }
        public Guid? HomeRoomId { get; set; }

    }
}
