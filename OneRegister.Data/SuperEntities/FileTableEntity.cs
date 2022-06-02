using System;

namespace OneRegister.Data.SuperEntities
{
    public class FileTableEntity : BaseEntity
    {
        public long? DmsId { get; set; }
        public Guid? DmsUrl { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
