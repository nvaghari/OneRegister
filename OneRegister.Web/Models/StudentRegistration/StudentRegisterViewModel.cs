using OneRegister.Domain.Model.StudentRegistration;
using System.Collections.Generic;

namespace OneRegister.Web.Models.StudentRegistration
{
    public class StudentRegisterViewModel : StudentRegisterModel
    {
        public Dictionary<string, string> Schools { get; set; }
        public Dictionary<string, string> Years { get; set; }
        public Dictionary<string, string> IdentityTypes { get; set; }
        public Dictionary<string, string> Nationalities { get; set; }
        public Dictionary<string, string> ClassNames { get; set; }
        public Dictionary<string, string> HomeRooms { get; set; }
    }
}
