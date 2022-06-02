using OneRegister.Data.Contract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.SuperEntities
{
    public enum AddressType
    {
        MailingAddress,
        ResidentialAddress
    }

    [Table(nameof(MemberAddress), Schema = SchemaNames.Base)]
    public class MemberAddress : BaseEntity
    {
        [StringLength(DataLength.ADDRESS)]
        public string Address { get; set; }

        public AddressType AddressType { get; set; }
        public Member Member { get; set; }
        public Guid MemberId { get; set; }
    }
}
