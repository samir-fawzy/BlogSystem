namespace BlogSystem.Api.Error
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? message { get; set; }
        public ApiErrorResponse(int statusCode,string? message = null)
        {
            StatusCode = statusCode;
            this.message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }
        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request you have made",
                401 => "You are UnAuthorize",
                404 => "Resourses Not Found",
                500 => "Internal Server Error",
                _ => null
            };

        }
    }
}
