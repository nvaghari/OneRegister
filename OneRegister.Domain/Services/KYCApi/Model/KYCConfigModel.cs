using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.KYCApi.Model
{
    public class KYCConfigModel
    {
        public string ApiUrl { get; set; }
        public string ProjectKey { get; set; }
        public string ProjectModel { get; set; }
    }
}
