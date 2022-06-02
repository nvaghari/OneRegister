namespace OneRegister.Core.Model.ControllerResponse
{
    public class SimpleResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public static SimpleResponse FailBecause(string message)
        {
            return new SimpleResponse
            {
                IsSuccessful = false,
                Message = message
            };
        }
        public static SimpleResponse Fail => new SimpleResponse { IsSuccessful = false };

        public static SimpleResponse Success()
        {
            return new SimpleResponse
            {
                IsSuccessful = true
            };
        }

        public static SimpleResponse Success(string message)
        {
            return new SimpleResponse
            {
                IsSuccessful = true,
                Message = message
            };
        }
    }
}