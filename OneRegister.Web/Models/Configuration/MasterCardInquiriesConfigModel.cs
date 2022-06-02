using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Models.Configuration
{
    public class MasterCardInquiriesConfigModel
    {
        public bool IsEnable { get; set; }
        public int MinutesToCheck { get; set; }
    }
}
