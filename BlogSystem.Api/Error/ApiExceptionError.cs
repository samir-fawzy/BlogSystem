namespace BlogSystem.Api.Error
{
    public class ApiExceptionError : ApiErrorResponse
    {
        private readonly string? details;

        public ApiExceptionError(int statusCode,string? message = null,string? details = null):base(statusCode,message)
        {
            this.details = details;
        }
    }
}
