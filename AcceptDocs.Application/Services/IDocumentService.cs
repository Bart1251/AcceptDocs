using AcceptDocs.SharedKernel.Dto;

namespace AcceptDocs.Application.Services
{
    public interface IDocumentService
    {
        int Create(AddDocumentDto dto);
        void Delete(int id);
        DocumentDto Get(int id);
        List<DocumentDto> GetAllForUserWithTypeAndFlow(int id);
        DocumentDto GetWithDetails(int id);
        void Update(UpdateDocumentDto dto);
    }
}
