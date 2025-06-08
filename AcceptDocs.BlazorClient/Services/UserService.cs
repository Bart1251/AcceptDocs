using AcceptDocs.SharedKernel.Dto;
using Newtonsoft.Json;

namespace AcceptDocs.BlazorClient.Services
{
    public interface IUserService
    {
        Task<UserDto> Get(int id);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto> Get(int id)
        {
            var response = await _httpClient.GetAsync("/api/User/" + id);
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserDto>(content);
            }
            return new UserDto();
        }
    }
}
