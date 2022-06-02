namespace OneRegister.Domain.Model.Dms
{
    public class DMS_ResultModel
    {
        public long? docId { get; set; }
        public string magicUrl { get; set; }
        public int retVal { get; set; }
        public string retMsg { get; set; }
    }
}
