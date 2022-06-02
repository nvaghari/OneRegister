using OneRegister.Data.Contract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneRegister.Data.SuperEntities
{
    public enum MemberFileType
    {
        Other,
        Photo,
        Identity
    }

    [Table(nameof(MemberFile), Schema = SchemaNames.Base)]
    public class MemberFile : FileTableEntity
    {
        public MemberFileType FileType { get; set; }

        public Member Member { get; set; }
        public Guid MemberId { get; set; }
    }
}
