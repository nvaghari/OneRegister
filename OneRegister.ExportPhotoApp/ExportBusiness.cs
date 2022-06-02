using OneRegister.ExportPhotoApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace OneRegister.ExportPhotoApp
{
    public class ExportBusiness
    {
        private FormMain FORM;
        private static string TOKEN;

        private static JsonSerializerOptions SerialaizeOption =>
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public ExportBusiness(FormMain form)
        {
            FORM = form;
        }

        public bool IsAuthorized(LoginModel loginModel)
        {
            var apiUrl = FORM.GetBaseUrl() + "/Export/GetToken";
            var client = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
            var response = client.PostAsync(apiUrl, content).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ApplicationException("URL is not correct");
            }
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                TOKEN = JsonSerializer.Deserialize<TokenModel>(responseContent, SerialaizeOption)?.Token;
                if (string.IsNullOrEmpty(TOKEN))
                {
                    throw new ApplicationException("Token is not provided");
                }
                return true;
            }
            return false;
        }

        public SchoolYearModel GetSchoolYears()
        {
            var apiUrl = FORM.GetBaseUrl() + "/Export/GetSchoolYears";
            var client = new HttpClient();
            var requestModel = new TokenModel { Token = TOKEN };
            var content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            var response = client.PostAsync(apiUrl, content).Result;
            var responseContent = GetResponseContent(response);
            if (response.IsSuccessStatusCode)
            {
                var model = JsonSerializer.Deserialize<SchoolYearModel>(responseContent, SerialaizeOption);
                return model;
            }
            else
            {
                throw new ApplicationException("Error Code " + response.StatusCode);
            }
        }

        internal static bool HasPermissionToPath(string selectedPath)
        {
            try
            {
                var d = Directory.CreateDirectory(selectedPath + "/temp");
                Directory.Delete(selectedPath + "/temp");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] GetPhoto(string dmsUrl, Guid photoId)
        {
            var apiUrl = dmsUrl + "/File/" + photoId.ToString();
            var client = new HttpClient();
            var response = client.GetAsync(apiUrl).Result;
            return response.Content.ReadAsByteArrayAsync().Result;
        }

        public ClassHomeroomModel GetClassAndHomerooms(string schoolId, string yearId)
        {
            var apiUrl = FORM.GetBaseUrl() + "/Export/GetClassHomerRooms";
            var client = new HttpClient();
            var requestModel = new ClassesRequestModel
            {
                Token = TOKEN,
                YearId = Convert.ToInt32(yearId),
                SchoolId = new Guid(schoolId)
            };
            var content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            var response = client.PostAsync(apiUrl, content).Result;
            var responseContent = GetResponseContent(response);
            var model = JsonSerializer.Deserialize<ClassHomeroomModel>(responseContent, SerialaizeOption);
            return model;
        }

        internal List<StudentModel> GetStudentList(StudentListRequestModel requestModel)
        {
            requestModel.Token = TOKEN;
            var apiUrl = FORM.GetBaseUrl() + "/Export/GetStudentList";
            var client = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            var response = client.PostAsync(apiUrl, content).Result;
            var responseContent = GetResponseContent(response);
            var model = JsonSerializer.Deserialize<List<StudentModel>>(responseContent, SerialaizeOption);
            return model;
        }

        internal StudentModel GetStudent(string icNumber)
        {
            var requestModel = new StudentRequestModel
            {
                IcNumber = icNumber,
                Token = TOKEN
            };

            var apiUrl = FORM.GetBaseUrl() + "/Export/GetStudent";
            var client = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            var response = client.PostAsync(apiUrl, content).Result;
            var responseContent = GetResponseContent(response);
            var model = JsonSerializer.Deserialize<StudentModel>(responseContent, SerialaizeOption);
            return model;
        }

        private string GetResponseContent(HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return content;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                FORM.SetBeforeLoginAppearance();
                throw new UnauthorizedAccessException("Your Session has Expired");
            }
            var model = JsonSerializer.Deserialize<SimpleResponse>(content, SerialaizeOption);
            throw new ApplicationException($"ErrorCode: {response.StatusCode} : {model.Message}");
        }
    }
}