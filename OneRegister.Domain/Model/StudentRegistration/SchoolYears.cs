using OneRegister.Core.Model;
using OneRegister.Core.Model.ControllerResponse;
using System.Collections.Generic;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class SchoolYears : SimpleResponse
    {
        public List<TextValueModel> Schools { get; set; }
        public List<TextValueModel> Years { get; set; }
    }
}
