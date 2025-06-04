using AcceptDocs.Domain.Models;

namespace AcceptDocs.Domain.Contracts
{
    public interface IDocumentRepository : IRepository<Document>
    {
        List<Document> GetAllForUserWithTypeAndFlow(int userId);
        Document GetWithTypeFlowAndUser(int id);
    }
}
