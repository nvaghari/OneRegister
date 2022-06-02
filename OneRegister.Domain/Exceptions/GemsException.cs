using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Exceptions
{
    public class GemsException : Exception
    {
        public GemsException(int errorNumber,string function)
        {
            ErrorNumber = errorNumber;
            Function = function;
        }
        public int ErrorNumber { get; set; }
        public string Function { get; set; }
        public string Code => ErrorNumber.ToString();
        public override string Source => $"GEMS Exception> {Function}";
    }
}
