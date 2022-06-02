using System.Collections.Generic;

namespace OneRegister.Core.Model.DataTablesModel
{
    public class DtReturn<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; }
    }
}
