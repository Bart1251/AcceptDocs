using NLog.Web;
using NLog;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using AcceptDocs.Infrastructure;
using AcceptDocs.Domain.Contracts;
using AcceptDocs.Application.Mappings;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AcceptDocs.Application.Services;
using AcceptDocs.Infrastructure.Repositories;
using AcceptDocs.WebAPI.Middleware;

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
                    options.UseSqlite("Data Source=/../database.db"));

                builder.Services.AddAutoMapper(typeof(AppMappingProfile));

                builder.Services.AddFluentValidationAutoValidation();
                //builder.Services.AddScoped<IValidator<!!DTO!!>, !!VALIDATOR!!>();

                builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
                builder.Services.AddScoped<DataSeeder>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IPositionLevelService, PositionLevelService>();
                builder.Services.AddScoped<IPositionLevelRepository, PositionLevelRepository>();
                builder.Services.AddScoped<IDocumentFlowService, DocumentFlowService>();
                builder.Services.AddScoped<IDocumentFlowRepository, DocumentFlowRepository>();
                builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
                builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();

                builder.Services.AddScoped<ExceptionMiddleware>();

                builder.Services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                        };
                    });

                builder.Services.AddAuthorization();

                builder.Services.AddCors(o => o.AddPolicy("AcceptDocs", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));

                var app = builder.Build();

                if (app.Environment.IsDevelopment()) {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                //middleware
                app.UseStaticFiles();
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.UseCors("AcceptDocs");

                using (var scope = app.Services.CreateScope()) {
                    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                    dataSeeder.Seed();
                }

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
