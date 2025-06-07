using AcceptDocs.SharedKernel.Dto;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AcceptDocs.BlazorClient.Services
{
    public interface IDocumentService
    {
        Task<bool> SubmitDocument(AddDocumentDto dto);
        Task<List<DocumentDto>> GetAllForUserWithTypeAndFlow(int userId);
        Task<bool> Delete(int id);
        Task<bool> Update(UpdateDocumentDto dto);
        Task<DocumentDto> GetWithDetails(int id);
        Task DownloadFile(string fileName);
    }

    public class DocumentService : IDocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _js;

        public DocumentService(HttpClient httpClient, IJSRuntime js)
        {
            _httpClient = httpClient;
            _js = js;
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
            var response = await _httpClient.GetAsync("api/Document/user/" + userId);
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

        public async Task<DocumentDto> GetWithDetails(int id)
        {
            var response = await _httpClient.GetAsync("api/Document/" + id);
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DocumentDto>(content);
            }
            return new DocumentDto();
        }

        public async Task<bool> Update(UpdateDocumentDto dto)
        {
            var content = new MultipartFormDataContent();

            if(dto.File is not null) {
                dto.FileName = dto.File.Name;
                var fileContent = new StreamContent(dto.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(dto.File.ContentType);
                content.Add(content: fileContent, name: "file", fileName: dto.File.Name);
            }

            var json = System.Text.Json.JsonSerializer.Serialize(dto);
            var jsonContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            content.Add(jsonContent, "document");

            var response = await _httpClient.PutAsync("api/Document/" + dto.DocumentId, content);
            return response.IsSuccessStatusCode;
        }

        public async Task DownloadFile(string fileName)
        {
            var response = await _httpClient.GetAsync("api/Document/download/" + fileName);

            if (!response.IsSuccessStatusCode) {
                return;
            }

            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

            await _js.InvokeVoidAsync("downloadFileFromBytes", fileName, contentType, fileBytes);
        }
    }
}
