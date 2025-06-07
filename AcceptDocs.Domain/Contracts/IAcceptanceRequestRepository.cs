using AcceptDocs.Domain.Models;

namespace AcceptDocs.Domain.Contracts
{
    public interface IAcceptanceRequestRepository : IRepository<AcceptanceRequest>
    {
        List<AcceptanceRequest> GetAllForUser(int id);
        AcceptanceRequest GetWithDetails(int id);
    }
}
