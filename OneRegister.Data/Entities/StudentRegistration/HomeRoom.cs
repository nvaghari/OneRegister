using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System.Collections.Generic;

namespace OneRegister.Data.Entities.StudentRegistration
{
    public class HomeRoom : Site
    {
        public ICollection<Student> Students { get; set; }
    }
}
