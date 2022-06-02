using Microsoft.AspNetCore.Http;
using OneRegister.Domain.Validation.Attributes;
using System;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantUploadFileModel
    {
        [CustomRequired]
        [File(MaxSize =10)]
        public IFormFile File { get; set; }
        [CustomRequired]
        public Guid? Mid { get; set; }
        [CustomRequired]
        public string Id { get; set; }
    }
}
