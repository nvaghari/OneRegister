using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.ExportPhotoApp.Model
{
    public class StudentModel
    {
        public Guid Id { get; set; }
        public string Ic { get; set; }
        public Guid PhotoId { get; set; }
    }
}
