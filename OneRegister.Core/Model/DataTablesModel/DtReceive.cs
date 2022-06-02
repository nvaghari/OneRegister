namespace OneRegister.Core.Model.DataTablesModel
{
    public class DtReceive
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Search Search { get; set; }
    }
    public class Search
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }
}
