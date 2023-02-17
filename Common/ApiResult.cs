namespace MobileBasedCashFlowAPI.Common
{
    public class ApiResult
    {

        public ApiResult(string statusCode, string message, object? data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public ApiResult(string statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public string StatusCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; } = string.Empty;
    }
}
