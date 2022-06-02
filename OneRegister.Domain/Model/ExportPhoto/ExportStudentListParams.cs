using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.ExportPhoto
{
    public class ExportStudentListParams : ExportTokenModel
    {

        public Guid SchoolId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? HomeRoomId { get; set; }
    }
}
