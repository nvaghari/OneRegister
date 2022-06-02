using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.ExportPhotoApp.Model
{
    public class StudentListRequestModel : TokenModel
    {
        public string SchoolId { get; set; }
        public string ClassId { get; set; }
        public string HomeRoomId { get; set; }
    }
}
