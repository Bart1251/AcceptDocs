using AcceptDocs.Application.Mappings;
using AcceptDocs.Application.Services;
using AcceptDocs.Application.Validators;
using AcceptDocs.BlazorServer.Helpers;
using AcceptDocs.Domain.Contracts;
using AcceptDocs.Infrastructure;
using AcceptDocs.Infrastructure.Repositories;
using AcceptDocs.SharedKernel.Dto;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Radzen;

namespace AcceptDocs.BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "../Log/nlog.config")).GetCurrentClassLogger();
            logger.Debug("init main");
            try {
                var builder = WebApplication.CreateBuilder(args);

                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.AddRazorPages();
                builder.Services.AddServerSideBlazor();

                builder.Services.AddAutoMapper(typeof(AppMappingProfile));

                builder.Services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite("Data Source=" + Directory.GetCurrentDirectory() + "/../database.db"));

                builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
                builder.Services.AddScoped<DataSeeder>();
                builder.Services.AddScoped<NavigationHelper>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IPositionLevelService, PositionLevelService>();
                builder.Services.AddScoped<IPositionLevelRepository, PositionLevelRepository>();
                builder.Services.AddScoped<IDocumentFlowService, DocumentFlowService>();
                builder.Services.AddScoped<IDocumentFlowRepository, DocumentFlowRepository>();
                builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
                builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
                builder.Services.AddScoped<IDocumentService, DocumentService>();
                builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
                builder.Services.AddScoped<IAcceptanceRequestService, AcceptanceRequestService>();
                builder.Services.AddScoped<IAcceptanceRequestRepository, AcceptanceRequestRepository>();

                builder.Services.AddScoped<IValidator<AddUserDto>, RegisterAddUserDtoValidator>();
                builder.Services.AddScoped<IValidator<UpdateUserDto>, RegisterUpdateUserDtoValidator>();
                builder.Services.AddScoped<IValidator<AddDocumentFlowDto>, RegisterAddDocumentFlowDtoValidator>();
                builder.Services.AddScoped<IValidator<UpdateDocumentFlowDto>, RegisterUpdateDocumentFlowDtoValidator>();
                builder.Services.AddScoped<IValidator<PositionLevelDto>, RegisterPositionLevelDtoValidator>();
                builder.Services.AddScoped<IValidator<DocumentTypeDto>, RegisterDocumentTypeDtoValidator>();

                builder.Services.AddRadzenComponents();

                var app = builder.Build();


                if (!app.Environment.IsDevelopment()) {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();

                app.UseStaticFiles();

                app.UseRouting();

                app.MapBlazorHub();
                app.MapFallbackToPage("/_Host");

                //using (var scope = app.Services.CreateScope()) {
                //    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                //    dataSeeder.Seed();
                //}

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