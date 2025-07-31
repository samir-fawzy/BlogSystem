using BlogSystem.Api.Error;
using BlogSystem.Api.Helper;
using BlogSystem.Core.Interfaces;
using BlogSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Extensions
{
    internal static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(MapProfile));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Where(P => P.Value.Errors.Count > 0)
                                                    .SelectMany(P => P.Value.Errors)
                                                    .Select(P => P.ErrorMessage)
                                                    .ToArray();
                    var validationErrorResponse = new ApiValidationError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            return services;
        }
    }
}
