using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Exceptions
{
    public class SecurityModuleException : Exception
    {
        public SecurityModuleException(string message): base(message)
        {
        }
    }
}
