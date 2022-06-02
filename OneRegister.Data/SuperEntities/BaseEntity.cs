using OneRegister.Data.Contract;
using System;
using System.ComponentModel.DataAnnotations;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.SuperEntities
{

    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = ModifiedAt = DateTime.Now;
        }
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Guid? ModifiedBy { get; set; }
        public Guid Id { get;set; }
        [StringLength(DataLength.Name)]
        [Required]
        public string Name { get; set; }
        public StateOfEntity State { get; set; }
    }
}
