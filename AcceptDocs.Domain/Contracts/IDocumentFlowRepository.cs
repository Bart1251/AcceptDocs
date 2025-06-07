using AcceptDocs.Domain.Models;

namespace AcceptDocs.Domain.Contracts
{
    public interface IDocumentFlowRepository : IRepository<DocumentFlow>
    {
        List<DocumentFlowUser> GetAttachedUsers(int id);
        void AttachUser(DocumentFlowUser documentFlowUser);
        void DetachUser(int documentFlowId, int userId);
        List<User> GetNotAttachedUsers(int id);
        List<DocumentFlow> GetAllWithUsers();
        DocumentFlowUser GetUserAttachment(int userId, int documentFlowId);
        bool CanChangeSelectionMethod(int id, SelectionMethod replacement);
    }
}
