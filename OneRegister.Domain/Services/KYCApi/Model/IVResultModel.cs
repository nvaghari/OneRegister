using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.KYCApi.Model
{
    public class IVResultModel
    {
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Nationality { get; set; }
        public string IssuingCountry { get; set; }
        public string ExpiryDate { get; set; }
        public string Address { get; set; }
        public string DocumentType { get; set; }
        public string DocumentImageUrl { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string LivenessResult { get; set; }
        public string FaceRecognitionResult { get; set; }
        public string DocumentVerificationResult { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
