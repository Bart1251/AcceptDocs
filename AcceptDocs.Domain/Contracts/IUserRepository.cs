using AcceptDocs.Domain.Models;

namespace AcceptDocs.Domain.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        List<User> GetAllWithPositionLevel();
        bool IsLoginUsed(string login);
        bool IsLoginUsed(string login, int id);
        User GetWithPositionLevel(int id);
        bool CanDeleteUser(int id);
    }
}
