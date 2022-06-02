using System;

namespace OneRegister.Domain.Model.Account
{
    public class RoleManagementListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public bool IsAssigned { get; set; }
    }
}
