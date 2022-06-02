using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.KYCApi.Model
{
    public enum DVDocumentType
    {
        MYKAD,
        PASSPORT
    }
    public class DVRequestModel
    {
        public string DocumentUri { get; set; }
        public string BackSideDocumentUri { get; set; }
        public string DocumentType { get; set; }

    }
}
