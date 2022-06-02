using System;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class SalesPersonListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AssignedNumber { get; set; }
    }
}
