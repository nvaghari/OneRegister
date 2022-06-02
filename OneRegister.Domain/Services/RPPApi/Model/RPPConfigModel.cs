using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.RPPApi.Model
{
    public class RPPConfigModel
    {
        public string ApiUrl { get; set; }
        public string SecretKey { get; set; }
        public int SourceId { get; set; }
    }
}
