namespace OneRegister.Web.Models.Dms
{
    public class GetPhotoModel
    {
        public string DomainName { get; set; }
        public long DocId { get; set; }
        public int AccessLevel { get; set; }
        public string DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int Silent { get; set; }
    }
}