using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.ExportPhoto
{
    public class ExportClassesModel : ExportTokenModel
    {
        public Guid SchoolId { get; set; }
        public int YearId { get; set; }
    }
}
