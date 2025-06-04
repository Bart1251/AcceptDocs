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
        public IDocumentRepository DocumentRepository { get; }

        //repositories

        public AppUnitOfWork(AppDbContext context, IUserRepository userRepository, IPositionLevelRepository positionLevelRepository, IDocumentFlowRepository documentFlowRepository, IDocumentTypeRepository documentTypeRepository, IDocumentRepository documentRepository)//repositories
        {
            _context = context;
            UserRepository = userRepository;
            PositionLevelRepository = positionLevelRepository;
            DocumentFlowRepository = documentFlowRepository;
            DocumentTypeRepository = documentTypeRepository;
            DocumentRepository = documentRepository;
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
