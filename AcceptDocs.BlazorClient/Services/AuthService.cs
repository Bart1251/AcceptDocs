using AcceptDocs.BlazorClient.Helpers;
using AcceptDocs.SharedKernel.Dto;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AcceptDocs.BlazorClient.Services
{
    public interface IAuthService
    {
        Task<bool> Login(UserLoginDto dto);
        Task Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Login(UserLoginDto dto)
        {
            var content = JsonConvert.SerializeObject(dto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Auth/login", bodyContent);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
            var token = data["token"];

            await _localStorageService.SetItemAsync<string>("authToken", token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _authStateProvider.GetAuthenticationStateAsync();
            return true;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;

            await _authStateProvider.GetAuthenticationStateAsync();
        }
    }
}
