using NLog.Web;
using NLog;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using AcceptDocs.Infrastructure;
using AcceptDocs.Domain.Contracts;
using AcceptDocs.Application.Mappings;

namespace AcceptDocs.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try {
                var builder = WebApplication.CreateBuilder(args);

                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("../database.db"));

                builder.Services.AddAutoMapper(typeof(AppMappingProfile));

                builder.Services.AddFluentValidationAutoValidation();
                //builder.Services.AddScoped<IValidator<!!DTO!!>, !!VALIDATOR!!>();

                builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
                //repositories
                //services
                //middlewares

                builder.Services.AddCors(o => o.AddPolicy("AcceptDocs", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }));

                var app = builder.Build();

                if (app.Environment.IsDevelopment()) {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                //middleware
                app.UseStaticFiles();
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.UseCors("AcceptDocs");

                app.Run();

            } catch (Exception exception) {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}
