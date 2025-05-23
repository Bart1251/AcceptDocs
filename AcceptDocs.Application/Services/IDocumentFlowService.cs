
using AcceptDocs.SharedKernel.Dto;

namespace AcceptDocs.Application.Services
{
    public interface IDocumentFlowService
    {
        int Create(AddDocumentFlowDto dto);
        List<DocumentFlowDto> GetAll();
        void Delete(int id);
        DocumentFlowDto Get(int id);
        void Update(UpdateDocumentFlowDto dto);
    }
}
