namespace OneRegister.Web.Models.Audit
{
    public class LogDataGrid
    {
        public string Time { get; set; }
        public string Level { get; set; }
        public string Session { get; set; }
        public string RemoteAddress { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
    }
}
