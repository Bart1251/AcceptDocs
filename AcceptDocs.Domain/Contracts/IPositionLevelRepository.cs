using AcceptDocs.Domain.Models;

namespace AcceptDocs.Domain.Contracts
{
    public interface IPositionLevelRepository : IRepository<PositionLevel>
    {
        bool CanDelete(int id);
    }
}
