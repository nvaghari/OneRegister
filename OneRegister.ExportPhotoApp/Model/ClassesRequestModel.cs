using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.ExportPhotoApp.Model
{
    public class ClassesRequestModel : TokenModel
    {
        public Guid SchoolId { get; set; }
        public int YearId { get; set; }
    }
}
