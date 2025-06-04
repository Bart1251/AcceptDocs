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
    }
}
