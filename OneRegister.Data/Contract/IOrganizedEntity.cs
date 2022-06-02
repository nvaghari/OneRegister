using OneRegister.Data.SuperEntities;
using System;

namespace OneRegister.Data.Contract
{
    public interface IOrganizedEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        DateTime CreatedAt { get; set; }

        Guid? CreatedBy { get; set; }
        DateTime ModifiedAt { get; set; }

        Guid? ModifiedBy { get; set; }
        StateOfEntity State { get; set; }

        Organization Organization { get; set; }
        Guid OrganizationId { get; set; }
    }
}
