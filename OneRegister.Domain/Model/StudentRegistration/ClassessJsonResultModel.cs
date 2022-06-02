using OneRegister.Core.Model;
using System.Collections.Generic;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class ClassessJsonResultModel
    {
        public List<TextValueModel> Classes { get; set; }
        public List<TextValueModel> HomeRooms { get; set; }
    }
}
