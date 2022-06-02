using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System.Collections.Generic;

namespace OneRegister.Data.Entities.StudentRegistration
{
    public class School : Organization
    {
        public ICollection<Student> Students { get; set; }

    }
}
