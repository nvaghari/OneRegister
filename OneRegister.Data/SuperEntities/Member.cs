using OneRegister.Data.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.SuperEntities
{
    public enum MemberType
    {
        Root = 0,
        Student = 1,
        MerchantOwner = 2,
        Agropreneur = 3,
        SalesPerson = 4
    }
    public enum Gender
    {
        Female = 0,
        Male = 1
    }

    [Table(nameof(Member), Schema = SchemaNames.Base)]
    public class Member : BaseEntity, IOrganizedEntity
    {
        public Member()
        {
            MemberFiles = new List<MemberFile>();
            MemberAddresses = new List<MemberAddress>();
        }
        public DateTime? BirthDay { get; set; }

        [StringLength(DataLength.SHORTNAME)]
        public string Designation { get; set; }

        [StringLength(DataLength.EMAIL)]
        public string Email { get; set; }

        [StringLength(DataLength.SHORTNAME)]
        public string FirstName { get; set; }

        public Gender Gender { get; set; }

        [StringLength(DataLength.IDENTITY)]
        public string IdentityNumber { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string IdentityType { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string LastName { get; set; }

        public ICollection<MemberAddress> MemberAddresses { get; set; }

        public ICollection<MemberFile> MemberFiles { get; set; }

        [StringLength(DataLength.PHONE)]
        public string Mobile { get; set; }

        [StringLength(DataLength.SHORTNAME)]
        public string Nationality { get; set; }
        public Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
