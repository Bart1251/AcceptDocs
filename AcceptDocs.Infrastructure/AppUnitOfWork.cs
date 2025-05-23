using AcceptDocs.Domain.Contracts;

namespace AcceptDocs.Infrastructure
{
    public class AppUnitOfWork : IAppUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository UserRepository { get; }
        public IPositionLevelRepository PositionLevelRepository { get; }

        //repositories

        public AppUnitOfWork(AppDbContext context, IUserRepository userRepository, IPositionLevelRepository positionLevelRepository)//repositories
        {
            _context = context;
            UserRepository = userRepository;
            PositionLevelRepository = positionLevelRepository;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
