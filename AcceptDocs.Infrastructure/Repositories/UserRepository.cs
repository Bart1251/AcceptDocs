using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }

        public List<User> GetAllWithPositionLevel()
        {
            return _context.Users.AsNoTracking().Include(u => u.PositionLevel).ToList();
        }

        public bool IsLoginUsed(string login)
        {
            return _context.Users.AsNoTracking().Any(u => u.Login == login);
        }

        public bool IsLoginUsed(string login, int id)
        {
            return _context.Users.AsNoTracking().Any(u => u.Login == login && u.UserId != id);
        }

        public User GetWithPositionLevel(int id)
        {
            return _context.Users.AsNoTracking().Include(u => u.PositionLevel).Where(u => u.UserId == id).First();
        }
    }
}
