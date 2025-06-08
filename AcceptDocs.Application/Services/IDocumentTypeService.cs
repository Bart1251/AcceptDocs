using AcceptDocs.SharedKernel.Dto;

namespace AcceptDocs.Application.Services
{
    public interface IDocumentTypeService
    {
        List<DocumentTypeDto> GetAll();
        DocumentTypeDto Get(int id);
        void Delete(int id);
        void Update(DocumentTypeDto dto);
        int Create(DocumentTypeDto dto);
        bool CanDelete(int id);
    }
}
