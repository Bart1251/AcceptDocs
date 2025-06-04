using AcceptDocs.SharedKernel.Dto;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;

namespace AcceptDocs.BlazorClient.Services
{
    public interface IDocumentService
    {
        Task<bool> SubmitDocument(AddDocumentDto dto);
        Task<List<DocumentDto>> GetAllForUserWithTypeAndFlow(int userId);
        Task<bool> Delete(int id);
    }

    public class DocumentService : IDocumentService
    {
        private readonly HttpClient _httpClient;

        public DocumentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SubmitDocument(AddDocumentDto dto)
        {
            dto.FileName = dto.File.Name;
            var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(dto.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(dto.File.ContentType);
            content.Add(content: fileContent, name: "file", fileName: dto.File.Name);

            var json = System.Text.Json.JsonSerializer.Serialize(dto);
            var jsonContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            content.Add(jsonContent, "document");

            var response = await _httpClient.PostAsync("api/Document", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<DocumentDto>> GetAllForUserWithTypeAndFlow(int userId)
        {
            var response = await _httpClient.GetAsync("api/Document/all/" + userId);
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DocumentDto>>(content);
            }
            return new List<DocumentDto>();
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync("api/Document/" + id);
            return response.IsSuccessStatusCode;
        }
    }
}
