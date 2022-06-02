using System;

namespace OneRegister.Domain.Model.Account
{
    public class UserListModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
        public string Phone { get; set; }
        public bool IsEnable { get; set; }
    }
}
