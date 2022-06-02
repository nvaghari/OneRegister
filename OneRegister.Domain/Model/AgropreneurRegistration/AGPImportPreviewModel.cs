using OneRegister.Core.Model.ControllerResponse;
using System.Collections.Generic;

namespace OneRegister.Domain.Model.AgropreneurRegistration
{
    public class AGPImportPreviewModel : SimpleResponse
    {
        public AGPImportPreviewModel()
        {
            Agps = new List<AGPImportModel>();
        }
        public static AGPImportPreviewModel Failure(string description)
        {
            return new AGPImportPreviewModel
            {
                IsSuccessful = false,
                Message = description
            };
        }
        public static AGPImportPreviewModel Success(List<AGPImportModel> records)
        {
            return new AGPImportPreviewModel
            {
                IsSuccessful = true,
                Agps = records
            };
        }
        public static AGPImportPreviewModel Success(string description)
        {
            return new AGPImportPreviewModel
            {
                IsSuccessful = true,
                Message = description
            };
        }
        public List<AGPImportModel> Agps { get; set; }
    }
}
