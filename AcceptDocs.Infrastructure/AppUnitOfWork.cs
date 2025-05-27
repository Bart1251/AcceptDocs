using AcceptDocs.Domain.Contracts;

namespace AcceptDocs.Infrastructure
{
    public class AppUnitOfWork : IAppUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository UserRepository { get; }
        public IPositionLevelRepository PositionLevelRepository { get; }
        public IDocumentFlowRepository DocumentFlowRepository { get; }
        public IDocumentTypeRepository DocumentTypeRepository { get; }

        //repositories

        public AppUnitOfWork(AppDbContext context, IUserRepository userRepository, IPositionLevelRepository positionLevelRepository, IDocumentFlowRepository documentFlowRepository, IDocumentTypeRepository documentTypeRepository)//repositories
        {
            _context = context;
            UserRepository = userRepository;
            PositionLevelRepository = positionLevelRepository;
            DocumentFlowRepository = documentFlowRepository;
            DocumentTypeRepository = documentTypeRepository;
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
