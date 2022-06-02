using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.RPPApi.ErrorHandling
{
    public class RPPException : Exception
    {
        public RPPException(HttpStatusCode status, string method)
        {
            Code = (int)status + " " + status.ToString();
            Method = method;
        }
        public RPPException(HttpStatusCode status, string method, string responseBody)
        {
            Code = (int)status + " " + status.ToString();
            Method = method;
            ResponseMessage = GetResponseMessage(responseBody);
        }

        private static string GetResponseMessage(string responseBody)
        {
            return responseBody;
        }

        public string Code { get; set; }
        public string Method { get; set; }
        public string ResponseMessage { get; set; }
        public override string Source => $"KYC API EXception> {Method}";
        public override string Message => ResponseMessage ?? Message;
    }
}
