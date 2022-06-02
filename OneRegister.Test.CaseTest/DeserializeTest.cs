using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRegister.Domain.Services.KYCApi.Model;
using OneRegister.Web.Models.MerchantRegistration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneRegister.Test.CaseTest
{
    [TestClass]
    public class DeserializeTest
    {
        private class SomeModel
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public int Some_Integer { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public decimal Some_Decimal { get; set; }
        }

        [TestMethod]
        public void InCaseOfAbscence()
        {
            var jsonStr = "{\"Setup_OneTime\":125.10}";

            var model = JsonSerializer.Deserialize<MerchantCommissionViewModel>(jsonStr);

            Assert.AreEqual(125.10M, model.Setup_OneTime);
        }

        [TestMethod]
        public void MaximumLengthEstimate()
        {
            var model = new SomeModel();
            var jsonStr = JsonSerializer.Serialize(model);
            Assert.IsTrue(jsonStr.Length < 4000);
        }

        [TestMethod]
        public void DynamicDeserialize()
        {
            string jsonResult = @"{
""status"": ""INTERNAL_SERVER_ERROR"",
    ""timestamp"": ""09-09-2021 11:00:24"",
    ""message"": ""documentUri is required.""
}";

            var result = JsonSerializer.Deserialize<ResponseErrorModel>(jsonResult,new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            Assert.IsTrue(result.Message == "documentUri is required.");

        }
        [TestMethod]
        public void JsonDocumentDeserialize()
        {
            string jsonResult = @"{
""status"": ""INTERNAL_SERVER_ERROR"",
    ""timestamp"": ""09-09-2019 11:00:24"",
    ""message"": ""documentUri is required.""
}";

            var jDoc = JsonDocument.Parse(jsonResult);
            if (jDoc.RootElement.TryGetProperty("message", out JsonElement message))
            {
                Assert.IsTrue(message.GetString() == "documentUri is required.");
            }
        }
    }
}