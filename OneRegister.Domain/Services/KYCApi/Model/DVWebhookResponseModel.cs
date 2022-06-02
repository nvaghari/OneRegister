namespace OneRegister.Domain.Services.KYCApi.Model
{
    public class DVWebhookResponseModel
    {
        public string UserKey { get; set; }
        public OnfidoResultForDV ReportDetails { get; set; }
    }
}
