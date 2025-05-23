using AcceptDocs.Domain.Models;

namespace AcceptDocs.Domain.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        public List<User> GetAllWithPositionLevel();
        public bool IsLoginUsed(string login);
        public bool IsLoginUsed(string login, int id);
        public User GetWithPositionLevel(int id);
    }
}
