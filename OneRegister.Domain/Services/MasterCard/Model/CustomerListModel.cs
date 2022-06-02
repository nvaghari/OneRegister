using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.Model
{
    public class CustomerListModel
    {
        public string FormNo { get; set; }
        public string FormType { get; set; }
        public string FormStatus { get; set; }
        public string Channel { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string IdNo { get; set; }
        public string EntityCIF { get; set; }
        public string ListServicePackages { get; set; }
    }
}
