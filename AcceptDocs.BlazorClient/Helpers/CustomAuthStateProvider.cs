using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace AcceptDocs.BlazorClient.Helpers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;
        public int? CurrentUserId { get; private set; }

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsync<string>("authToken");
            var identity = new ClaimsIdentity();
            CurrentUserId = null;

            if (!string.IsNullOrEmpty(token)) {
                var handler = new JwtSecurityTokenHandler();

                JwtSecurityToken jwt;

                try {
                    jwt = handler.ReadJwtToken(token);
                } catch {
                    await _localStorageService.RemoveItemAsync("authToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == "exp");

                if (expClaim != null && long.TryParse(expClaim.Value, out long exp)) {
                    var expiryDate = DateTimeOffset.FromUnixTimeSeconds(exp);
                    if (expiryDate > DateTimeOffset.UtcNow) {
                        identity = new ClaimsIdentity(jwt.Claims, "jwt");
                        CurrentUserId = int.Parse(jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    } else {
                        await _localStorageService.RemoveItemAsync("authToken");
                    }
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return new AuthenticationState(user);
        }
    }
}
