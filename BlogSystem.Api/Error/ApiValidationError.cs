namespace BlogSystem.Api.Error
{
    public class ApiValidationError : ApiErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationError():base(400)
        {
            Errors = new List<string>();
        }
    }
}
