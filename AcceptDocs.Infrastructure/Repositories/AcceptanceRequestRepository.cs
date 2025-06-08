using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class AcceptanceRequestRepository : Repository<AcceptanceRequest>, IAcceptanceRequestRepository
    {
        public AcceptanceRequestRepository(AppDbContext context) : base(context)
        {
        }

        public AcceptanceRequest GetWithDetails(int id)
        {
            return _context.AcceptanceRequests.Where(ar => ar.AcceptanceRequestId == id)
                .Include(ar => ar.User)
                .Include(ar => ar.Document)
                    .ThenInclude(d => d.DocumentFlow)
                .FirstOrDefault()!;
        }

        public List<AcceptanceRequest> GetAllForUser(int id)
        {
            return _context.AcceptanceRequests.Where(ar => ar.UserId == id)
                .Include(ar => ar.User)
                .Include(ar => ar.Document)
                    .ThenInclude(d => d.DocumentFlow)
                .Include(ar => ar.Document)
                    .ThenInclude(d => d.DocumentType)
                .OrderByDescending(ar => ar.Document.CreatedAt)
                .ToList();
        }
    }
}
