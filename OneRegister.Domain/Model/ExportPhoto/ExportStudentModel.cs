using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.ExportPhoto
{
    public class ExportStudentModel
    {
        public Guid Id { get; set; }
        public string Ic { get; set; }
        public Guid PhotoId { get; set; }
    }
}
