using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
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
                .First(df => df.DocumentFlowId == id).DocumentFlowUsers.ToList();
        }

        public void AttachUser(DocumentFlowUser documentFlowUser)
        {
            _context.DocumentFlowUsers.Add(documentFlowUser);
        }

        public void DetachUser(int documentFlowId, int userId)
        {
            var documentFlowUser = _context.DocumentFlowUsers.First(dfu => dfu.UserId == userId && dfu.DocumentFlowId == documentFlowId);
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
            return _context.DocumentFlowUsers.First(dfu => dfu.UserId == userId && dfu.DocumentFlowId == documentFlowId);
        }
    }
}
