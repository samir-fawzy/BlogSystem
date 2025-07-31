
using BlogSystem.Api.Extensions;
using BlogSystem.Api.Middlewares;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Repository.Data;
using BlogSystem.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogSystem.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // ******************* DbContext *********************//
            // Add Identity DbContext
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
            // Add Blog DbContext
            builder.Services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // ****************************************************//
            // ************ Services ************* //
            builder.Services.AddIdentityServices(builder.Configuration); // Custome Extension Method 
            builder.Services.AddApplicationService();
            // *********************************** //


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            #region Updata Database
            var scoped = app.Services.CreateScope();
            var services = scoped.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var BlogIdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await BlogIdentityDbContext.Database.MigrateAsync();

                var BlogDbContext = services.GetRequiredService<BlogDbContext>();
                await BlogDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error occured during Migration");
            }
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionErrorMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseStatusCodePagesWithRedirects("/error/{0}");

            app.MapControllers();

            app.Run();
        }
    }
}
