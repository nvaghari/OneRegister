namespace OneRegister.Core.Model.Dms
{
    public class InsertPhotoModel
    {
        public string DomainName { get; set; }
        public string RefId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public int DslGlobal { get; set; }
        public int DslDomain { get; set; }
        public int DslOwner { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int Silent { get; set; }
    }
}
