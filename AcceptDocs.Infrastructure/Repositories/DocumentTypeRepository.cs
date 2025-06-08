using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class DocumentTypeRepository : Repository<DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(AppDbContext context) : base(context)
        {
        }

        public bool CanDelete(int id)
        {
            var documentType = _context.DocumentTypes.Include(dt => dt.Documents).FirstOrDefault(dt => dt.DocumentTypeId == id);
            if (documentType == null)
                return true;
            return documentType.Documents.Count == 0;
        }
    }
}
