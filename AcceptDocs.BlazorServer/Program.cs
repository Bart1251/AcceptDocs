using AcceptDocs.Application.Mappings;
using AcceptDocs.Application.Services;
using AcceptDocs.Application.Validators;
using AcceptDocs.Domain.Contracts;
using AcceptDocs.Infrastructure;
using AcceptDocs.Infrastructure.Repositories;
using AcceptDocs.SharedKernel.Dto;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace AcceptDocs.BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddAutoMapper(typeof(AppMappingProfile));

            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("Data Source=/../database.db"));

            builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPositionLevelService, PositionLevelService>();
            builder.Services.AddScoped<IPositionLevelRepository, PositionLevelRepository>();
            builder.Services.AddScoped<IDocumentFlowService, DocumentFlowService>();
            builder.Services.AddScoped<IDocumentFlowRepository, DocumentFlowRepository>();

            builder.Services.AddScoped<IValidator<AddUserDto>, RegisterAddUserDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateUserDto>, RegisterUpdateUserDtoValidator>();
            builder.Services.AddScoped<IValidator<AddDocumentFlowDto>, RegisterAddDocumentFlowDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateDocumentFlowDto>, RegisterUpdateDocumentFlowDtoValidator>();

            builder.Services.AddRadzenComponents();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            using (var scope = app.Services.CreateScope()) {
                var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                dataSeeder.Seed();
            }

            app.Run();
        }
    }
}