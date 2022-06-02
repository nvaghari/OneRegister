using System;

namespace OneRegister.Domain.Model.Account
{
    public class RoleListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public int AssignedNumber { get; set; }
    }
}
