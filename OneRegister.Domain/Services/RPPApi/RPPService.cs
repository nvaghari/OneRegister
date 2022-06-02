using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Services.MasterCard.Model;
using OneRegister.Domain.Services.RPPApi.ErrorHandling;
using OneRegister.Domain.Services.RPPApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.RPPApi
{
    public class RPPService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RPPService> _logger;
        private readonly IMapper _mapper;

        public RPPService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<RPPService> logger,
            IMapper mapper
            )
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _mapper = mapper;
        }
        public RPPConfigModel Config
        {
            get
            {
                var model = new RPPConfigModel();
                _configuration.GetSection("Services:RPP").Bind(model);
                return model;
            }
        }

        public BankAccountResponseModel CheckBankAccount(CheckBankAccountModel domainModel)
        {

            domainModel.CalculateHash(Config.SecretKey,Config.SourceId);
            var apiModel = _mapper.Map<BankAccountSendModel>(domainModel);
            var payload = JsonSerializer.Serialize(apiModel);
            HttpClient client = _httpClientFactory.CreateClient();
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            _logger.LogInformation($"-> [RPP] Request To API: {payload}");
            var response = client.PostAsync(Config.ApiUrl + "/AMMBRPP", content).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            _logger.LogInformation($"<- [RPP] Response {response.StatusCode} {responseBody}");
            if (!response.IsSuccessStatusCode)
            {
                throw new RPPException(response.StatusCode, "AMMBRPP", responseBody);
            }
            return JsonSerializer.Deserialize<BankAccountResponseModel>(responseBody);

        }
    }
}
