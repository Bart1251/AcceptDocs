using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class DocumentTypeRepository : Repository<DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
