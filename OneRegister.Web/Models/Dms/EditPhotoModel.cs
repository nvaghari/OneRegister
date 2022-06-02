namespace OneRegister.Web.Models.Dms
{
    public class EditPhotoModel
    {
        public long DocId { get; set; }
        public string DomainName { get; set; }
        public string RefId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public int DslGlobal { get; set; }
        public int DslDomain { get; set; }
        public int DslOwner { get; set; }
        public string ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public int Silent { get; set; }
    }
}
