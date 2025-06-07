using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class DocumentFlowRepository : Repository<DocumentFlow>, IDocumentFlowRepository
    {
        public DocumentFlowRepository(AppDbContext context) : base(context)
        {
        }

        public List<DocumentFlow> GetAllWithUsers()
        {
            return _context.DocumentFlows.Include(df => df.DocumentFlowUsers).ThenInclude(dfu => dfu.User).ToList();
        }

        public List<DocumentFlowUser> GetAttachedUsers(int id)
        {
            return _context.DocumentFlows
                .Include(df => df.DocumentFlowUsers)
                    .ThenInclude(dfu => dfu.User)
                .FirstOrDefault(df => df.DocumentFlowId == id)?.DocumentFlowUsers.ToList()!;
        }

        public void AttachUser(DocumentFlowUser documentFlowUser)
        {
            _context.DocumentFlowUsers.Add(documentFlowUser);
        }

        public void DetachUser(int documentFlowId, int userId)
        {
            var documentFlowUser = _context.DocumentFlowUsers.FirstOrDefault(dfu => dfu.UserId == userId && dfu.DocumentFlowId == documentFlowId);
            if(documentFlowUser != null)
                _context.DocumentFlowUsers.Remove(documentFlowUser);
        }

        public List<User> GetNotAttachedUsers(int id)
        {
            return _context.Users
                .Include(u => u.DocuemntFlowUsers)
                .Include(u => u.PositionLevel)
                .Where(u => !u.DocuemntFlowUsers.Any(dfu => dfu.DocumentFlowId == id))
                .ToList();
        }

        public DocumentFlowUser GetUserAttachment(int userId, int documentFlowId)
        {
            return _context.DocumentFlowUsers.FirstOrDefault(dfu => dfu.UserId == userId && dfu.DocumentFlowId == documentFlowId)!;
        }

        public bool CanChangeSelectionMethod(int id, SelectionMethod replacement)
        {
            var documentFlow = _context.DocumentFlows.Include(df => df.Documents).FirstOrDefault(df => df.DocumentFlowId == id);
            if (documentFlow == null)
                return true;
            if (documentFlow.SelectionMethod == replacement)
                return true;
            return documentFlow.Documents.Count(d => d.Status == DocumentStatus.WaitingForApproval) == 0;
        }
    }
}
