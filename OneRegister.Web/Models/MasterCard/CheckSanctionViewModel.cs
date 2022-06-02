using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Models.MasterCard
{
    public class CheckSanctionViewModel
    {
        public string FirstName { get; set; }
        [CustomRequired]
        public string LastName { get; set; }
        public string BirthDay { get; set; }

    }
}
