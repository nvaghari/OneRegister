using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.StudentRegistration
{
    public class ClassRoom : Site
    {
        [StringLength(DataLength.SHORTNAME)]
        public string Label { get; set; }


        public ICollection<Student> Students { get; set; }

    }
}
