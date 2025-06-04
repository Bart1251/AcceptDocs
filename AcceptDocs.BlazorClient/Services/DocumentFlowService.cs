using AcceptDocs.SharedKernel.Dto;
using Newtonsoft.Json;

namespace AcceptDocs.BlazorClient.Services
{
    public interface IDocumentFlowService
    {
        Task<List<DocumentFlowDto>> GetAll();
    }

    public class DocumentFlowService : IDocumentFlowService
    {
        private readonly HttpClient _httpClient;

        public DocumentFlowService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DocumentFlowDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("api/DocumentFlow");
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DocumentFlowDto>>(content);
            }
            return new List<DocumentFlowDto>();
        }
    }
}
