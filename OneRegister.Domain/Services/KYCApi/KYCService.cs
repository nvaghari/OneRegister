using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneRegister.Domain.Services.KYCApi.ErrorHandling;
using OneRegister.Domain.Services.KYCApi.Model;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace OneRegister.Domain.Services.KYCApi
{
    public class KYCService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<KYCService> _logger;
        public static JsonSerializerOptions SerializeOption => new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        public KYCService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<KYCService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public KYCConfigModel Config
        {
            get
            {
                return _configuration.GetSection("Services:KYC").Get<KYCConfigModel>();
            }
        }
        private string GetToken()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("ProjectKey", Config.ProjectKey);
            client.DefaultRequestHeaders.Add("ProjectModel", Config.ProjectModel);
            _logger.LogDebug("[KYC] Sending Request For token...");
            _logger.LogDebug($"ProjectKey {Config.ProjectKey} ProjectModel {Config.ProjectModel}");
            var response = client.GetAsync(Config.ApiUrl + "/getToken").Result;
            var token = response.Headers.GetValues("token").FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError($"[KYC] There is no token in header response code: {response.StatusCode}");
            }
            _logger.LogDebug("[KYC] Authentication Token: " + token);
            return token;

        }
        private string GetToken(string projectModel, string projectKey)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("ProjectKey", projectKey);
            client.DefaultRequestHeaders.Add("ProjectModel", projectModel);
            _logger.LogDebug("[KYC] Sending Request For token...");
            _logger.LogDebug($"ProjectKey {projectKey} ProjectModel {projectModel}");
            var response = client.GetAsync(Config.ApiUrl + "/getToken").Result;
            var token = response.Headers.GetValues("token").FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError($"[KYC] There is no token in header response code: {response.StatusCode}");
            }
            _logger.LogDebug("[KYC] Authentication Token: " + token);
            return token;

        }

        internal string GetSanctionscreening(SSRequestModel model)
        {
            var client = GetKYCClient();
            var payload = JsonSerializer.Serialize(model, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _logger.LogInformation($"-> [KYC] Request to SS: {payload}");
            string queryString = GetSSQueryString(model);
            var response = client.PostAsync(Config.ApiUrl + "/watchlist-report?" + queryString, null).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            _logger.LogInformation($"<- [KYC] Response from SS {response.StatusCode} {responseBody}");
            if (!response.IsSuccessStatusCode)
            {
                throw new KycException(response.StatusCode, "watchlist-report", responseBody);
            }
            //var resultModel = JsonSerializer.Deserialize<SSResultModel>(responseBody, _serializeOption);
            //if (string.IsNullOrEmpty(resultModel.CheckId))
            //{
            //    throw new KycException(response.StatusCode, "watchlist-report", responseBody);
            //}
            return responseBody;
        }

        private static string GetSSQueryString(SSRequestModel model)
        {
            StringBuilder sb = new();
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                sb.Append("firstName=" + WebUtility.UrlEncode(model.FirstName) + "&");
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                sb.Append("lastName=" + WebUtility.UrlEncode(model.LastName) + "&");
            }
            if (!string.IsNullOrEmpty(model.BirthDate))
            {
                sb.Append("birthDate=" + WebUtility.UrlEncode(model.BirthDate) + "&");
            }

            return sb.ToString();
        }

        public string GetDocumentVerification(DVRequestModel model)
        {
            var client = GetKYCClient();
            var payload = JsonSerializer.Serialize(model, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            _logger.LogInformation($"-> [KYC] Request to DV: {payload}");
            var response = client.PostAsync(Config.ApiUrl + "/validatingDocAndBackSide", content).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            _logger.LogInformation($"<- [KYC] Response from DV {response.StatusCode} {responseBody}");
            if (!response.IsSuccessStatusCode)
            {
                throw new KycException(response.StatusCode, "validatingDocAndBackSide", responseBody);
            }

            return responseBody;
        }
        public string CheckSanctionScreening(string firstName, string lastName, string birthDay)
        {
            var model = new SSRequestModel
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDay
            };
            var client = GetKYCClient("OneRegisterDev", "8c7008c9ba9fdddf565e12799e2fab75be0c233031aa7a2ac20930a7a41abf01b07b4ad0c20dbd9c07eaf02056834498");
            var payload = JsonSerializer.Serialize(model, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            string queryString = GetSSQueryString(model);
            var response = client.PostAsync(Config.ApiUrl + "/watchlist-report?" + queryString, null).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                return responseBody;
            }
            var resultModel = JsonSerializer.Deserialize<SSResultModel>(responseBody, SerializeOption);
            if (string.IsNullOrEmpty(resultModel.CheckId))
            {
                throw new System.Exception("cannot find the checkId in response");
            }

            string resultResponseBody = GetWatchListReport(client, resultModel.CheckId);

            var result = JsonSerializer.Deserialize<OnfidoReportResult>(resultResponseBody, SerializeOption);
            if (!string.IsNullOrEmpty(result.Status) && result.Status == "awaiting_data")
            {
                //just a simple recurring job to avoid webhook
                for (int i = 1; i < 4; i++)
                {
                    Thread.Sleep(1000 * 5 * i);
                    resultResponseBody = GetWatchListReport(client, resultModel.CheckId);
                    result = JsonSerializer.Deserialize<OnfidoReportResult>(resultResponseBody, SerializeOption);
                    if (!string.IsNullOrEmpty(result.Status) && result.Status == "complete") return resultResponseBody;
                }
            }
            return resultResponseBody;
        }

        private string GetWatchListReport(HttpClient client, string checkId)
        {
            var resultResponse = client.GetAsync(Config.ApiUrl + "/watchlist-report-result?checkID=" + checkId).Result;
            var resultResponseBody = resultResponse.Content.ReadAsStringAsync().Result;
            return resultResponseBody;
        }

        internal string GetUserKeyFromDVResult(string kycResultStr)
        {
            var resultModel = JsonSerializer.Deserialize<DVResultModel>(kycResultStr, SerializeOption);
            if (string.IsNullOrEmpty(resultModel.UserKey))
            {
                throw new KycException(HttpStatusCode.OK, "validatingDocAndBackSide", kycResultStr);
            }

            return resultModel.UserKey;
        }

        private HttpClient GetKYCClient()
        {
            var token = GetToken();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("ProjectKey", Config.ProjectKey);
            client.DefaultRequestHeaders.Add("ProjectModel", Config.ProjectModel);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return client;

        }
        private HttpClient GetKYCClient(string projectModel, string projectKey)
        {
            var token = GetToken(projectModel, projectKey);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("ProjectKey", projectKey);
            client.DefaultRequestHeaders.Add("ProjectModel", projectModel);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return client;

        }
        public IVResultModel GetIVReport(string userKey)
        {
            var client = GetKYCClient();
            _logger.LogInformation($"-> [KYC] Request to IV: userKey={userKey}");
            var response = client.GetAsync(Config.ApiUrl + "/ekycResult?userKey=" + userKey).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            _logger.LogInformation($"<- [KYC] Response from IV {response.StatusCode} {responseBody}");
            if (!response.IsSuccessStatusCode)
            {
                throw new KycException(response.StatusCode, "ekycResult", responseBody);
            }
            return JsonSerializer.Deserialize<IVResultModel>(responseBody, SerializeOption);
        }
    }
}
