using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(AppDbContext context) : base(context)
        {
        }

        public List<Document> GetAllForUserWithTypeAndFlow(int userId)
        {
            return _context.Documents.Where(d => d.UserId == userId)
                .Include(d => d.DocumentType)
                .Include(d => d.DocumentFlow).ToList();
        }

        public Document GetWithDetails(int id)
        {
            return _context.Documents.Where(d => d.DocumentId == id)
                .Include(d => d.AcceptanceRequests)
                    .ThenInclude(ar => ar.User)
                        .ThenInclude(u => u.PositionLevel)
                .Include(d => d.DocumentType)
                .Include(d => d.User)
                .Include(d => d.DocumentFlow).FirstOrDefault()!;
        }
    }
}
