using System;

namespace OneRegister.Data.Contract;

public interface IBaseEntity
{
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }
    public StateOfEntity State { get; set; }
}
