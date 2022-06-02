using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.ExportPhoto
{
    public class ExportStudentParams : ExportTokenModel
    {
        public string IcNumber { get; set; }
    }
}
