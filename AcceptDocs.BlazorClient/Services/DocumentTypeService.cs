using AcceptDocs.SharedKernel.Dto;
using Newtonsoft.Json;

namespace AcceptDocs.BlazorClient.Services
{
    public interface IDocumentTypeService
    {
        Task<List<DocumentTypeDto>> GetAll();
    }


    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly HttpClient _httpClient;

        public DocumentTypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DocumentTypeDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/DocumentType");
            if(response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DocumentTypeDto>>(content);
            }
            return new List<DocumentTypeDto>();
        }
    }
}
