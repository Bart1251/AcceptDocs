using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class DocumentFlowRepository : Repository<DocumentFlow>, IDocumentFlowRepository
    {
        public DocumentFlowRepository(AppDbContext context) : base(context)
        {
        }
    }
}
