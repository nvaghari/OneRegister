using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.Model
{
    public class MCTaskListGridModel
    {
        public string Id { get; set; }
        public string RefId { get; set; }
        public string RefId2 { get; set; }
        public string Name { get; set; }
        public string InquiryName { get; set; }
        public string Source { get; set; }
        public string CreatedAt { get; set; }
        public string ModifiedAt { get; set; }
        public string Result { get; set; }
        public string ErrorSource { get; set; }
        public string State { get; set; }
        public string ErrorCode { get; set; }
    }
}
