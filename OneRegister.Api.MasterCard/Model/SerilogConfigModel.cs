using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Api.MasterCard.Model
{
    public class SerilogConfigModel
    {
        private string _path = string.Empty;
        public string Path
        {
            get
            {
                return _path.EndsWith("/") ? _path : _path += "/";
            }
            set
            {
                _path = value;
            }
        }
        public string FileName { get; set; }
        public string Format { get; set; }
        public AuditLevel AuditLevel { get; set; }
    }
    public class AuditLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string Serilog { get; set; }
    }
}
