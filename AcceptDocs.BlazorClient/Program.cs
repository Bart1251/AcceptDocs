using AcceptDocs.Application.Validators;
using AcceptDocs.BlazorClient.Helpers;
using AcceptDocs.BlazorClient.Services;
using AcceptDocs.SharedKernel.Dto;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

namespace AcceptDocs.BlazorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IDocumentFlowService, DocumentFlowService>();
            builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();

            builder.Services.AddScoped<NavigationHelper>();

            builder.Services.AddScoped<IValidator<AddDocumentDto>, RegisterAddDocumentDtoValidator>();

            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<CustomAuthStateProvider>());
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("APIUrl")) });

            builder.Services.AddRadzenComponents();

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}
