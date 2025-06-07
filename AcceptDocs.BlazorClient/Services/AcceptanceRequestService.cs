using AcceptDocs.SharedKernel.Dto;
using Newtonsoft.Json;
using System.Text;

namespace AcceptDocs.BlazorClient.Services
{
    public interface IAcceptanceRequestService
    {
        Task<List<AcceptanceRequestDto>> GetAllForUser(int id);
        Task<bool> GiveFeedback(AddDocumentFeedbackDto dto);
        Task<AcceptanceRequestDto> Get(int id);
    }

    public class AcceptanceRequestService : IAcceptanceRequestService
    {
        private readonly HttpClient _httpClient;
        public AcceptanceRequestService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<AcceptanceRequestDto> Get(int id)
        {
            var response = await _httpClient.GetAsync("api/AcceptanceRequest/" + id);
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AcceptanceRequestDto>(content);
            }
            return new AcceptanceRequestDto();
        }

        public async Task<List<AcceptanceRequestDto>> GetAllForUser(int id)
        {
            var response = await _httpClient.GetAsync("api/AcceptanceRequest/user/" + id);
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AcceptanceRequestDto>>(content);
            }
            return new List<AcceptanceRequestDto>();
        }

        public async Task<bool> GiveFeedback(AddDocumentFeedbackDto dto)
        {
            var content = JsonConvert.SerializeObject(dto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/AcceptanceRequest/" + dto.AcceptanceRequestId, bodyContent);
            return response.IsSuccessStatusCode;
        }

        
    }
}
