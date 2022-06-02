using Microsoft.AspNetCore.Http;
using OneRegister.Domain.Validation.Attributes;

namespace OneRegister.Domain.Model.Shared
{
    public class ImportFile
    {
        [CustomRequired]
        [ExcelFile]
        public IFormFile File { get; set; }
    }
}
