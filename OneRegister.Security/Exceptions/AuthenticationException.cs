using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Exceptions
{
    public class AuthenticationException : Exception
    {
        public string FriendlyMessage { get; set; } = "User Authentication was failed";
        public AuthenticationException(string message, Exception exception = null, string friendlyMessage = null)
            : base(message, exception)
        {
            if (friendlyMessage != null)
            {
                FriendlyMessage = friendlyMessage;
            }
        }
    }
}
