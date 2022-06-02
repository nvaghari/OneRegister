using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.Configuration;
using OneRegister.Domain.Model.Dms;
using System;
using System.Net.Http;
using System.Text.Json;

namespace OneRegister.Domain.Services.Dms
{
    public class DmsService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public DmsService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory
            )
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public string GetFileUrl(Guid? dmsUrl)
        {
            DmsApiConfig config = GetConfig();
            if (dmsUrl.HasValue)
            {
                return config.ApiUrl + "File/" + dmsUrl.Value.ToString();
            }
            return "/pic/unknown.jpg";
        }
        public string GetFileUrl(FileTableEntity file)
        {
            if (file == null)
            {
                return "/pic/unknown.jpg";
            }
            DmsApiConfig config = GetConfig();
            if (file.DmsUrl == null)
            {
                return "/pic/unknown.jpg";
            }
            return config.ApiUrl + "File/" + file.DmsUrl.Value.ToString();
        }
        public (long docId, Guid url) InsertFile(IFormFile file, Guid? id = null)
        {
            DmsApiConfig config = GetConfig();
            var client = _httpClientFactory.CreateClient();

            var apiUrlToAdd = config.LocalApiUrl + "File/Add";

            var content = new MultipartFormDataContent();

            content.Headers.Add("user", config.UserName);
            content.Headers.Add("key", config.Password);

            var fileContent = new ByteArrayContent(file.ToByte());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "File", file.FileName);
            content.Add(new StringContent(id.HasValue ? id.Value.ToString() : string.Empty), "RefId");
            content.Add(new StringContent(string.Empty), "RefId2");
            content.Add(new StringContent(string.Empty), "RefId3");
            content.Add(new StringContent("0"), "DSL_Global");
            content.Add(new StringContent("7"), "DSL_Domain");
            content.Add(new StringContent("7"), "DSL_Owner");

            var response = client.PostAsync(apiUrlToAdd, content).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            DMS_ResultModel result = JsonSerializer.Deserialize<DMS_ResultModel>(responseBody);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"DMS API Error ({result.retVal}) {result.retMsg}");
            }
            return (result.docId.Value, new Guid(result.magicUrl));
        }
        public (long docId, Guid url) UpdateFile(long docId, IFormFile file)
        {
            DmsApiConfig config = GetConfig();
            var client = _httpClientFactory.CreateClient();

            var apiUrlToAdd = config.LocalApiUrl + "File/Update";

            var content = new MultipartFormDataContent();

            content.Headers.Add("user", config.UserName);
            content.Headers.Add("key", config.Password);

            var fileContent = new ByteArrayContent(file.ToByte());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "File", file.FileName);

            content.Add(new StringContent(docId.ToString()), "DocId");
            content.Add(new StringContent("0"), "NewDSL_Global");
            content.Add(new StringContent("7"), "NewDSL_Domain");
            content.Add(new StringContent("7"), "NewDSL_Owner");

            var response = client.PostAsync(apiUrlToAdd, content).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            DMS_ResultModel result = JsonSerializer.Deserialize<DMS_ResultModel>(responseBody);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"DMS API Error ({result.retVal}) {result.retMsg}");
            }
            return (result.docId.Value, new Guid(result.magicUrl));
        }

        private DmsApiConfig GetConfig()
        {
            var model = new DmsApiConfig();
            _configuration.GetSection("Services:DMS").Bind(model);
            if (string.IsNullOrEmpty(model.ApiUrl))
            {
                throw new ApplicationException("DMS API URL is not provided");
            }
            if (string.IsNullOrEmpty(model.LocalApiUrl))
            {
                throw new ApplicationException("Local DMS API URL is not provided");
            }
            if (string.IsNullOrEmpty(model.UserName))
            {
                throw new ApplicationException("DMS API UserName is not provided");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ApplicationException("DMS API Password is not provided");
            }
            model.ApiUrl = model.ApiUrl.EndsWith('/') ? model.ApiUrl : model.ApiUrl + "/";
            model.LocalApiUrl = model.LocalApiUrl.EndsWith('/') ? model.LocalApiUrl : model.LocalApiUrl + "/";
            return model;
        }
    }
}
