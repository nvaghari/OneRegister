using Microsoft.AspNetCore.Http;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class ImportModel
    {
        public IFormFile CsvFile { get; set; }
    }
}
