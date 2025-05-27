
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
        List<DocumentFlowUserDto> GetAttachedUsers(int id);
        void AttachUser(AttachUserDto dto);
        void DetachUser(int documentFlowId, int userId);
        List<UserDto> GetNotAttachedUsers(int id);
        List<DocumentFlowDto> GetAllWithUsers();
        void UpdateAttachedUserValue(AttachUserDto dto);
    }
}
