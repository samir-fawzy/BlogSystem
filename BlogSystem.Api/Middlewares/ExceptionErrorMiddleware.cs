using BlogSystem.Api.Error;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json;

namespace BlogSystem.Api.Middlewares
{
    public class ExceptionErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionErrorMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionErrorMiddleware(RequestDelegate next,ILogger<ExceptionErrorMiddleware> logger,IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex) 
            {
                // log error
                logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // Handle Exception 
                var response = env.IsDevelopment() ?
                    new ApiExceptionError((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    :
                    new ApiExceptionError((int)HttpStatusCode.InternalServerError);
                var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, option);
                await context.Response.WriteAsync(json);

            }
        }
    }
}
