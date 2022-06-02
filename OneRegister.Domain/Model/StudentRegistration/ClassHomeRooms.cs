using OneRegister.Core.Model;
using OneRegister.Core.Model.ControllerResponse;
using System.Collections.Generic;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class ClassHomeRooms : SimpleResponse
    {
        public List<TextValueModel> Classes { get; set; }
        public List<TextValueModel> HomeRooms { get; set; }
    }
}
