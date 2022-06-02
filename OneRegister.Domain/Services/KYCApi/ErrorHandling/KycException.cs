using OneRegister.Domain.Services.KYCApi.Model;
using System;
using System.Net;
using System.Text.Json;

namespace OneRegister.Domain.Services.KYCApi.ErrorHandling
{
    public class KycException : Exception
    {
        private JsonSerializerOptions _serializeOption => new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        public KycException(HttpStatusCode status, string method)
        {
            Code = (int)status + " " + status.ToString();
            Method = method;
        }
        public KycException(HttpStatusCode status, string method, string responseBody)
        {
            Code = (int)status + " " + status.ToString();
            Method = method;
            ResponseMessage = GetResponseMessage(responseBody);
        }

        private string GetResponseMessage(string responseBody)
        {
            var model = JsonSerializer.Deserialize<ResponseErrorModel>(responseBody, _serializeOption);
            return model.Message ?? string.Empty;
        }

        public string Code { get; set; }
        public string Method { get; set; }
        public string ResponseMessage { get; set; }
        public override string Source => $"KYC API EXception> {Method}";
        public override string Message => ResponseMessage ?? Message;
    }
}
