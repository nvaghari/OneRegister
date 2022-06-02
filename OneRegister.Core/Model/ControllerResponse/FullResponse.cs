using System.Collections.Generic;

namespace OneRegister.Core.Model.ControllerResponse
{
    public class FullResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public FullResponse()
        {
            Validations = new List<ValidationModel>();
        }

        public static FullResponse SuccessWithId(string id)
        {
            return new FullResponse
            {
                IsSuccessful = true,
                Id = id
            };
        }
        public static FullResponse FailBecause(string reason)
        {
            return new FullResponse
            {
                IsSuccessful = false,
                Message = reason
            };
        }
        public static FullResponse Success => new() { IsSuccessful = true };

        public List<ValidationModel> Validations { get; set; }
        public string Id { get; set; }
    }
}